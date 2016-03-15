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

        private static readonly RSACryptoServiceProvider RSAProvider;

        private static readonly String GlobalPrivateKey;

        private static readonly String GlobalPublicKey;

        private static readonly DESCryptoServiceProvider DESProvider;

        private static readonly Byte[] GlobalIV;

        private static readonly String GlobalIVBase64String;

        #endregion

        #region Constructors

        static AccountController()
        {
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
            AccountController.RSAProvider = rsaProvider;
            AccountController.GlobalPrivateKey = rsaProvider.ToXmlString(true);
            AccountController.GlobalPublicKey = rsaProvider.ToXmlString(false);

            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
            AccountController.DESProvider = desProvider;
            AccountController.GlobalIV = desProvider.IV;
            AccountController.GlobalIVBase64String = Convert.ToBase64String(AccountController.GlobalIV);
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
            response.Headers.Add("NE", AccountController.GlobalPublicKey);
            response.Headers.Add("IV", AccountController.GlobalIVBase64String);
            return response;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Login")]
        public HttpResponseMessage Login()
        {
            HttpResponseMessage response;
            AccountSecurityContext securityContext = CreateSecurityContext(base.ActionContext, out response);
            if (securityContext != null)
            {
                KeyValuePair<String, String> account;
                if (TryGetAccountAndPassword(base.ActionContext, securityContext, out account))
                {
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotAcceptable;
                    response.ReasonPhrase = "invalid account and password.";
                }
            }
            return response;
        }

        #region Private

        /// <summary>
        /// 获取认证信息。
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="securityContext"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        private Boolean TryGetAccountAndPassword(HttpActionContext actionContext, AccountSecurityContext securityContext,out KeyValuePair<String, String> account)
        {
            AuthenticationHeaderValue authenticationHeader = actionContext.Request.Headers.Authorization;
            if (authenticationHeader != null)
            {
                String arg = authenticationHeader.Parameter;
                if (!String.IsNullOrEmpty(arg))
                {
                    String temp = securityContext.Dencrypt(arg);
                    String[] accountAndPassword = temp.Split(':');
                    if (accountAndPassword.Length == 2)
                    {
                        account = new KeyValuePair<String, String>(accountAndPassword[0], accountAndPassword[1]);
                        return true;
                    }
                }
            }
            account = new KeyValuePair<String, String>();
            return false;
        }

        /// <summary>
        /// 创建通信的安全上下文。
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private AccountSecurityContext CreateSecurityContext(HttpActionContext actionContext, out HttpResponseMessage response)
        {
            String secretKey = GetClientSecretKey(base.ActionContext, out response);
            if (!String.IsNullOrEmpty(secretKey))
            {
                Byte[] secretKeyData = Convert.FromBase64String(secretKey);
                ICryptoTransform decryptor = AccountController.DESProvider.CreateDecryptor(secretKeyData, AccountController.GlobalIV);
                ICryptoTransform encryptor = AccountController.DESProvider.CreateEncryptor(secretKeyData, AccountController.GlobalIV);
                return new AccountSecurityContext(encryptor, decryptor);
            }
            return null;
        }

        /// <summary>
        /// 获取客户端设置密钥。
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private String GetClientSecretKey(HttpActionContext actionContext, out HttpResponseMessage response)
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            String reasonPhrase = null;
            String secretKey = null;
            HttpRequestHeaders headers = actionContext.Request.Headers;
            //negotiatory-encryption:协商加密
            if (headers.Contains("NE"))
            {
                headers.GetValues("NE");
                String temp = headers.GetValues("NE").First();
                Byte[] data = Convert.FromBase64String(temp);
                AccountController.RSAProvider.FromXmlString(AccountController.GlobalPrivateKey);
                try
                {
                    data = AccountController.RSAProvider.Decrypt(data, true);
                    secretKey = Encoding.UTF8.GetString(data);
                }
                catch (CryptographicException)
                {
                    statusCode = HttpStatusCode.NotAcceptable;
                    reasonPhrase = "Invalid base-64 string.";
                }
            }
            else
            {
                statusCode = HttpStatusCode.BadRequest;
                reasonPhrase = "can not found key 'NE'.";
            }
            response = new HttpResponseMessage(statusCode);
            response.ReasonPhrase = reasonPhrase;
            return secretKey;
        }

        #endregion

        #endregion
    }
}
