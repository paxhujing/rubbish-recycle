using RubbishRecycle.Controllers.Assets;
using RubbishRecycle.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Web.Http;
using System.Linq;
using System.Text;
using System.Web.Http.Controllers;
using System.Net;
using System.Collections.Generic;

namespace RubbishRecycle.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        #region Fields

        #region 非对称加密

        private static readonly RSACryptoServiceProvider RSAProvider;

        private static readonly String GlobalPrivateKey;

        private static readonly String GlobalPublicKey;

        #endregion
        
        #region 对称加密

        private static readonly DESCryptoServiceProvider DESProvider;

        private static readonly Byte[] GlobalIV;

        private static readonly String GlobalIVBase64String;

        #endregion

        private static readonly MD5CryptoServiceProvider MD5Provider;

        #endregion

        #region Constructors

        static AccountController()
        {
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
            AccountController.RSAProvider = rsaProvider;
            AccountController.GlobalPrivateKey = rsaProvider.ToXmlString(true);
            AccountController.RSAProvider.FromXmlString(AccountController.GlobalPrivateKey);
            AccountController.GlobalPublicKey = rsaProvider.ToXmlString(false);

            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
            AccountController.DESProvider = desProvider;
            AccountController.GlobalIV = desProvider.IV;
            AccountController.GlobalIVBase64String = Convert.ToBase64String(AccountController.GlobalIV);

            AccountController.MD5Provider = new MD5CryptoServiceProvider();
        }

        public AccountController()
        {

        }

        #endregion

        #region Methods

        [AllowAnonymous]
        [HttpGet]
        [Route("RequestCommunication")]
        public HttpResponseMessage RequestCommunication()
        {
            HttpResponseMessage response = base.ActionContext.Request.CreateResponse (System.Net.HttpStatusCode.OK);
            //negotiatory-encryption:协商加密
            response.Headers.Add("Token", AccountController.GlobalPublicKey);
            return response;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Login")]
        public HttpResponseMessage Login()
        {
            HttpResponseMessage response;
            KeyValuePair<String, String> account;
            if (TryGetAccountAndPassword(base.ActionContext, out account,out response))
            {
                //验证用户
                if (VerifyAccount(account.Key, account.Value))
                {
                    String secretKey = GetClientSecretKey(base.ActionContext, out response);
                    if (!String.IsNullOrEmpty(secretKey))
                    {
                        AccountToken token = CreateAccountToken(secretKey, account.Key, account.Value);
                        AccountTokenManager.Manager.Add(token);
                        response.Headers.Add("Token", token.Token);
                        response.Headers.Add("IV", AccountController.GlobalIVBase64String);
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotAcceptable;
                    response.ReasonPhrase = "verify account failed.";
                }
            }
            return response;
        }

        #region Private

        /// <summary>
        /// 获取认证信息。
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="account"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private Boolean TryGetAccountAndPassword(HttpActionContext actionContext, out KeyValuePair<String, String> account, out HttpResponseMessage response)
        {
            response = new HttpResponseMessage(HttpStatusCode.OK);
            AuthenticationHeaderValue authenticationHeader = actionContext.Request.Headers.Authorization;
            if (authenticationHeader != null)
            {
                String arg = authenticationHeader.Parameter;
                if (!String.IsNullOrEmpty(arg))
                {
                    try
                    {
                        Byte[] data = Convert.FromBase64String(arg);
                        data = AccountController.RSAProvider.Decrypt(data, true);
                        String temp = Encoding.UTF8.GetString(data);
                        String[] accountAndPassword = temp.Split(':');
                        if (accountAndPassword.Length != 2)
                        {
                            throw new FormatException();
                        }
                        account = new KeyValuePair<String, String>(accountAndPassword[0], accountAndPassword[1]);
                        return true;
                    }
                    catch (ArgumentNullException)
                    {
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.ReasonPhrase = "account can not be empty.";
                    }
                    catch (FormatException)
                    {
                        response.StatusCode = HttpStatusCode.NotAcceptable;
                        response.ReasonPhrase = "account format error.";
                    }
                    catch (CryptographicException)
                    {
                        response.StatusCode = HttpStatusCode.NotAcceptable;
                        response.ReasonPhrase = "decryption failed.";
                    }
                }
            }
            else
            {
                response.StatusCode = HttpStatusCode.NonAuthoritativeInformation;
                response.ReasonPhrase = "Must set 'Authorization'.";
            }
            account = new KeyValuePair<String, String>();
            return false;
        }

        /// <summary>
        /// 验证用户信息。
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private Boolean VerifyAccount(String account, String password)
        {
            return true;
        }

        /// <summary>
        /// 获取客户端设置密钥。
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private String GetClientSecretKey(HttpActionContext actionContext, out HttpResponseMessage response)
        {
            response = new HttpResponseMessage(HttpStatusCode.OK);
            String secretKey = null;
            HttpRequestHeaders headers = actionContext.Request.Headers;
            //negotiatory-encryption:协商加密
            if (headers.Contains("Token"))
            {
                headers.GetValues("Token");
                String temp = headers.GetValues("Token").FirstOrDefault();
                try
                {
                    Byte[] data = Convert.FromBase64String(temp);
                    data = AccountController.RSAProvider.Decrypt(data, true);
                    secretKey = Encoding.UTF8.GetString(data);
                }
                catch (ArgumentNullException)
                {
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.ReasonPhrase = "value can not be empty.";
                }
                catch (FormatException)
                {
                    response.StatusCode = HttpStatusCode.NotAcceptable;
                    response.ReasonPhrase = "value is not base-64 format.";
                }
                catch (CryptographicException)
                {
                    response.StatusCode = HttpStatusCode.NotAcceptable;
                    response.ReasonPhrase = "decryption failed.";
                }
            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.ReasonPhrase = "can not found key 'Token'.";
            }
            return secretKey;
        }

        /// <summary>
        /// 创建通信的安全上下文。
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private AccountToken CreateAccountToken(String secretKey, String account, String password)
        {
            try
            {
                Byte[] secretKeyData = Convert.FromBase64String(secretKey);
                //创建安全上下文
                ICryptoTransform decryptor = AccountController.DESProvider.CreateDecryptor(secretKeyData, AccountController.GlobalIV);
                ICryptoTransform encryptor = AccountController.DESProvider.CreateEncryptor(secretKeyData, AccountController.GlobalIV);
                AccountSecurityContext context = new AccountSecurityContext(encryptor, decryptor);
                //生成令牌
                String temp = String.Format("{0}-{1}-{2}-{3}", secretKey, account, password, DateTime.Now.Ticks);
                Byte[] data = Encoding.UTF8.GetBytes(temp);
                data = AccountController.MD5Provider.ComputeHash(data);
                String token = Encoding.UTF8.GetString(data).Replace("-", String.Empty);

                AccountToken accountToken = new AccountToken(token, context);
                return accountToken;
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
        }

        #endregion

        #endregion
    }
}
