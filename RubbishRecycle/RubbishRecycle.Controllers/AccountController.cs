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
using RubbishRecycle.Toolkit;
using RubbishRecycle.Config;

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

        //private static readonly String GlobalIVBase64String;

        #endregion

        private static readonly MD5CryptoServiceProvider MD5Provider;

        #endregion

        #region Constructors

        static AccountController()
        {
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(2048);
            AccountController.RSAProvider = rsaProvider;
            AccountController.GlobalPrivateKey = rsaProvider.ToXmlString(true);
            AccountController.RSAProvider.FromXmlString(AccountController.GlobalPrivateKey);
            AccountController.GlobalPublicKey = rsaProvider.ToXmlString(false);

            RijndaelManaged aesProvider = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            AccountController.AESProvider = aesProvider;
            AccountController.GlobalIV = aesProvider.IV;
            //AccountController.GlobalIVBase64String = Convert.ToBase64String(AccountController.GlobalIV);

            AccountController.MD5Provider = new MD5CryptoServiceProvider();
        }

        public AccountController()
        {

        }

        #endregion

        #region Methods

        #region Actions

        [AllowAnonymous]
        [HttpGet]
        [Route("RequestCommunication")]
        public String RequestCommunication()
        {
            return AccountController.GlobalPublicKey;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetVerifyCode")]
        public VerifyCodeSmsResult GetVerifyCode([FromBody]String encryptedJson)
        {
            String json = AccountController.RSAProvider.Decrypt(encryptedJson);
            VerifyCodeRequest request = JsonConvert.DeserializeObject<VerifyCodeRequest>(json);
            return TaoBaoSms.SendVerifyCode(request.BindingPhone, request.RoleId);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public LoginResult Login([FromBody]String encryptedJson)
        {
            String json = AccountController.RSAProvider.Decrypt(encryptedJson);
            LoginInfo loginInfo = JsonConvert.DeserializeObject<LoginInfo>(json);
            Byte[] secretKey = loginInfo.SecretKey;
            if ((secretKey != null) && (secretKey.Length != 0))
            {
                //验证用户
                Account account = VerifyAccount(loginInfo.Name, loginInfo.Password);
                if (account != null)
                {
                    return InitAccountToken(secretKey, account); ;
                }
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RegisterSaler")]
        public LoginResult RegisterSaler([FromBody]String encryptedJson)
        {
            String json = AccountController.RSAProvider.Decrypt(encryptedJson);
            RegisterInfo registerInfo = JsonConvert.DeserializeObject<RegisterInfo>(json);
            Byte[] secretKey = registerInfo.SecretKey;
            if ((secretKey != null) && (secretKey.Length != 0))
            {
                Account account = null;
                if (TryRegisterAccount(registerInfo, "saler",out account))
                {
                    return InitAccountToken(secretKey, account); ;
                }
            }
            throw new HttpResponseException(HttpStatusCode.NotAcceptable);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RegisterBuyer")]
        public LoginResult RegisterBuyer([FromBody]String encryptedJson)
        {
            String json = AccountController.RSAProvider.Decrypt(encryptedJson);
            RegisterInfo registerInfo = JsonConvert.DeserializeObject<RegisterInfo>(json);
            Byte[] secretKey = registerInfo.SecretKey;
            if ((secretKey != null) && (secretKey.Length != 0))
            {
                Account account = null;
                if (TryRegisterAccount(registerInfo, "buyer", out account))
                {
                    return InitAccountToken(secretKey, account); ;
                }
            }
            throw new HttpResponseException(HttpStatusCode.NotAcceptable);
        }

        [RubbishRecycleAuthorize]
        public Account GetAccount()
        {
            AccountToken token = (AccountToken)base.ActionContext.Request.Properties["Token"];
            using (RubbishRecycleContext context = new RubbishRecycleContext())
            {
                return context.Accounts.First(x => x.Id == token.AccountId);
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// 注册用户信息。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="isSaler"></param>
        /// <returns></returns>
        private Boolean TryRegisterAccount(RegisterInfo registerInfo, String roleId, out Account account)
        {
            account = null;
            using (RubbishRecycleContext context = new RubbishRecycleContext())
            {
                account = new Account();
                account.RoleId = roleId;
                account.Id = Guid.NewGuid().GetHashCode();
                account.Name = registerInfo.Name;
                account.BindingPhone = registerInfo.BindingPhone;
                account.Password = MD5Compute(registerInfo.Password);
                account.LastLogin = DateTime.Now;
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
                Account account = context.Accounts.FirstOrDefault(x => ((x.Name == name) || (x.BindingPhone == name)) && (x.Password == MD5Compute(password)));
                return account;
            }
        }

        /// <summary>
        /// 加密登陆响应消息。
        /// </summary>
        /// <param name="secretKey">创建Token使用的客户端密钥。</param>
        /// <param name="account">客户端账号信息。</param>
        /// <returns>加密消息。</returns>
        private String EncryptLoginResponseMessage(Byte[] secretKey, Account account)
        {
            LoginResult result = InitAccountToken(secretKey, account);
            String json = JsonConvert.SerializeObject(result);
            return AccountController.RSAProvider.Encrypt(json);
        }

        /// <summary>
        /// 初始化账户令牌。
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="account"></param>
        /// <param name="accountType"></param>
        /// <returns></returns>
        private LoginResult InitAccountToken(Byte[] secretKey, Account account)
        {
            AccountToken accountToken = CreateAccountToken(secretKey, account);
            AccountTokenManager.Manager.Add(accountToken);
            LoginResult result = new LoginResult();
            result.Token = accountToken.Token;
            result.IV = AccountController.GlobalIV;
            return result;
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
                AESCryptor cryptor = new AESCryptor(encryptor, decryptor);
                //生成令牌
                String temp = String.Format("{0}{1}{2}{3}", secretKey.GetHashCode(), DateTime.Now.Ticks, account.Name, account.Password);
                String token = MD5Compute(temp);
                AccountToken accountToken = new AccountToken(token, account.Id, cryptor);
                accountToken.Role = account.RoleId;
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

        #region Misc

        /// <summary>
        /// 计算字符串的MD5值。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private String MD5Compute(String str)
        {
            Byte[] data = Encoding.UTF8.GetBytes(str);
            data = AccountController.MD5Provider.ComputeHash(data);
            return Convert.ToBase64String(data);
        }

        #endregion

        #endregion
    }
}
