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
using System.Xml;
using System.Data.Entity;

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
        public HttpResponseMessage Login(String encryptedJson)
        {
            String json = RSADecrypt(encryptedJson);
            LoginInfo loginInfo = JsonConvert.DeserializeObject<LoginInfo>(json);
            Byte[] secretKey = loginInfo.SecretKey;
            if ((secretKey != null) && (secretKey.Length != 0))
            {
                //验证用户
                Account account = VerifyAccount(loginInfo.Name, loginInfo.Password);
                if (account != null)
                {
                    //生成Token
                    return InitAccountToken(secretKey, account);
                }
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RegisterSaler")]
        public HttpResponseMessage RegisterSaler(String encryptedJson)
        {
            String json = RSADecrypt(encryptedJson);
            RegisterInfo registerInfo = JsonConvert.DeserializeObject<RegisterInfo>(json);
            Byte[] secretKey = registerInfo.SecretKey;
            if ((secretKey != null) && (secretKey.Length != 0))
            {
                Account account = null;
                if (TryRegisterAccount(registerInfo, AccountType.Saler,out account))
                {
                    return InitAccountToken(secretKey, account);
                }
            }
            throw new HttpResponseException(HttpStatusCode.NotAcceptable);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RegisterBuyer")]
        public HttpResponseMessage RegisterBuyer(String encryptedJson)
        {
            String json = RSADecrypt(encryptedJson);
            RegisterInfo registerInfo = JsonConvert.DeserializeObject<RegisterInfo>(json);
            Byte[] secretKey = registerInfo.SecretKey;
            if ((secretKey != null) && (secretKey.Length != 0))
            {
                Account account = null;
                if (TryRegisterAccount(registerInfo, AccountType.Buyer, out account))
                {
                    return InitAccountToken(secretKey, account);
                }
            }
            throw new HttpResponseException(HttpStatusCode.NotAcceptable);
        }

        #region Private

        /// <summary>
        /// 注册用户信息。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="isSaler"></param>
        /// <returns></returns>
        private Boolean TryRegisterAccount(RegisterInfo registerInfo, AccountType accountType, out Account account)
        {
            account = null;
            using (RubbishRecycleContext context = new RubbishRecycleContext())
            {
                String roleName = accountType == AccountType.Saler ? "saler" : "buyer";
                Role role = context.Roles.FirstOrDefault(x => x.RoleName == roleName);
                if (role == null) return false;
                AccountRole ar = new AccountRole();
                ar.Role = role;
                account = new Account();
                ar.Account = account;
                account.Id = Guid.NewGuid().GetHashCode();
                account.Name = registerInfo.Name;
                account.BindingPhone = registerInfo.BindingPhone;
                Byte[] data = Encoding.UTF8.GetBytes(registerInfo.Password);
                data = AccountController.MD5Provider.ComputeHash(data);
                account.Password = Convert.ToBase64String(data);
                account.LastLogin = DateTime.Now;
                account.AccountRoles.Add(ar);
                context.Accounts.Add(account);
                Int32 result = context.SaveChanges();
                return result != 0;
            }
        }

        /// <summary>
        /// 验证用户信息。
        /// </summary>
        /// <param name="name">账户名。</param>
        /// <param name="password">密码。</param>
        /// <returns></returns>
        private Account VerifyAccount(String name, String password)
        {
            using (RubbishRecycleContext context = new RubbishRecycleContext())
            {
                Account account = context.Accounts.FirstOrDefault(x => x.Name == name && x.Password == password);
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
            //String[] roles = account.Roles.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            //if (roles.Length != 0)
            //{
            //    foreach (String role in roles)
            //    {
            //        accountToken.Roles.Add(role);
            //    }
            //}
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
                    String temp = RSADecrypt(arg);
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
        /// RSA解密。
        /// </summary>
        /// <param name="encrypted">RSA加密后的BASE64数据</param>
        /// <returns>RSA解密后的数据。</returns>
        private String RSADecrypt(String encrypted)
        {
            Byte[] data = Convert.FromBase64String(encrypted);
            data = AccountController.RSAProvider.Decrypt(data, false);
            String temp = Encoding.UTF8.GetString(data);
            return temp;
        }

        /// <summary>
        /// 获取客户端设置密钥。
        /// </summary>
        /// <param name="e ncryptedSecretKey">加密后的客户端密钥。</param>
        /// <returns>解密后的客户端密钥。</returns>
        //private Byte[] GetClientSecretKey(String encryptedSecretKey)
        //{
        //    Byte[] data = Convert.FromBase64String(encryptedSecretKey);
        //    data = AccountController.RSAProvider.Decrypt(data, false);
        //    return data;
        //}

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
