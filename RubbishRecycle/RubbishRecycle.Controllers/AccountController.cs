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
using Newtonsoft.Json;
using RubbishRecycle.Controllers.Assets.DB;
using System.Xml;

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

        private static readonly RijndaelManaged AESProvider;

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

            RijndaelManaged aesProvider = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            AccountController.AESProvider = aesProvider;
            AccountController.GlobalIV = aesProvider.IV;
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
        public String RequestCommunication()
        {
            return AccountController.GlobalPublicKey;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public HttpResponseMessage Login([FromBody]String encryptedSecretKey,[FromUri]String role)
        {
            Byte[] secretKey = GetClientSecretKey(encryptedSecretKey);
            if (secretKey != null)
            {
                KeyValuePair<String, String> accountInfo;
                if (TryGetAccountAndPassword(base.ActionContext, out accountInfo))
                {
                    //验证用户
                    Account account = VerifyAccount(accountInfo.Key, accountInfo.Value, role);
                    if (account != null)
                    {
                        //生成Token
                        return InitAccountToken(secretKey, account);
                    }
                }
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public HttpResponseMessage Register([FromBody]String encryptedSecretKey, [FromUri]String role)
        {
            if (role == "saler" || role == "buyer")
            {
                Byte[] secretKey = GetClientSecretKey(encryptedSecretKey);
                if (secretKey != null)
                {
                    KeyValuePair<String, String> accountInfo;
                    if (TryGetAccountAndPassword(base.ActionContext, out accountInfo))
                    {
                        Account account = RegisterAccount(accountInfo.Key, accountInfo.Value, role);
                        //验证用户
                        if (account != null)
                        {
                            return InitAccountToken(secretKey, account);
                        }
                    }
                }
            }
            throw new HttpResponseException(HttpStatusCode.NotAcceptable);
        }

        [RubbishRecycleAuthorize(Roles ="saler")]
        [HttpGet]
        [Route("GetAccountInfo")]
        public String GetAccountInfo([FromUri]String name)
        {
            using (RubbishRecycleContext context = new RubbishRecycleContext())
            {
                Account account = context.Accounts.FirstOrDefault(x=>x.Name == name);
                if (account == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                AccountToken accountToken = (AccountToken)base.ActionContext.Request.Properties["AccountToken"];
                String json = JsonConvert.SerializeObject(account);
                return accountToken.SecurityContext.Encrypt(json);
            }
        }

        #region Private

        /// <summary>
        /// 注册用户信息。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        private Account RegisterAccount(String name, String password,String role)
        {
            using (RubbishRecycleContext context = new RubbishRecycleContext())
            {
                Account account = new Account();
                account.Name = name;
                account.Password = password;
                account.Role = role;
                account.LastLogin = DateTime.Now;
                context.Accounts.Add(account);
                Int32 result = context.SaveChanges();
                return result != 0 ? account : null;
            }
        }

        /// <summary>
        /// 验证用户信息。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        private Account VerifyAccount(String name, String password,String role)
        {
            using (RubbishRecycleContext context = new RubbishRecycleContext())
            {
                Account account = context.Accounts.FirstOrDefault(x => x.Name == name && x.Password == password && x.Role == role);
                return account;
            }
        }

        /// <summary>
        /// 初始化账户令牌。
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="account"></param>
        /// <param name="accountType"></param>
        /// <returns></returns>
        private HttpResponseMessage InitAccountToken(Byte[] secretKey, Account account)
        {
            AccountToken accountToken = CreateAccountToken(secretKey, account);
            accountToken.Roles.Add(account.Role);
            AccountTokenManager.Manager.Add(accountToken);
            HttpResponseMessage response = base.ActionContext.Request.CreateResponse(accountToken.Token);
            response.Headers.Add("IV", AccountController.GlobalIVBase64String);
            return response;
        }

        /// <summary>
        /// 获取认证信息。
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="account"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private Boolean TryGetAccountAndPassword(HttpActionContext actionContext, out KeyValuePair<String, String> account)
        {
            AuthenticationHeaderValue authenticationHeader = actionContext.Request.Headers.Authorization;
            if (authenticationHeader != null)
            {
                String arg = authenticationHeader.Parameter;
                if (!String.IsNullOrEmpty(arg))
                {
                    Byte[] data = Convert.FromBase64String(arg);
                    data = AccountController.RSAProvider.Decrypt(data, false);
                    String temp = Encoding.UTF8.GetString(data);
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
        /// 获取客户端设置密钥。
        /// </summary>
        /// <param name="encryptedSecretKey">加密后的客户端密钥。</param>
        /// <returns>解密后的客户端密钥。</returns>
        private Byte[] GetClientSecretKey(String encryptedSecretKey)
        {
            Byte[] data = Convert.FromBase64String(encryptedSecretKey);
            data = AccountController.RSAProvider.Decrypt(data, false);
            return data;
        }

        /// <summary>
        /// 创建通信的安全上下文。
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private AccountToken CreateAccountToken(Byte[] secretKey, Account account)
        {
            try
            {
                //创建安全上下文
                ICryptoTransform decryptor = AccountController.AESProvider.CreateDecryptor(secretKey, AccountController.GlobalIV);
                ICryptoTransform encryptor = AccountController.AESProvider.CreateEncryptor(secretKey, AccountController.GlobalIV);
                AccountSecurityContext context = new AccountSecurityContext(encryptor, decryptor);
                //生成令牌
                String temp = String.Format("{0}-{1}-{2}:{3}", secretKey.GetHashCode(), DateTime.Now.Ticks, account.Name, account.Password);
                Byte[] data = Encoding.UTF8.GetBytes(temp);
                data = AccountController.MD5Provider.ComputeHash(data);
                String token = Convert.ToBase64String(data);
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
