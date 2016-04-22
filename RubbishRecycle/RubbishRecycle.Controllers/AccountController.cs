using RubbishRecycle.Controllers.Assets;
using RubbishRecycle.Models;
using System;
using System.Security.Cryptography;
using System.Web.Http;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using RubbishRecycle.Toolkit;
using RubbishRecycle.Repositories;
using RubbishRecycle.Controllers.Repositories;

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

        #endregion

        private static readonly MD5CryptoServiceProvider MD5Provider;

        private readonly IAccountRepository<RubbishRecycleContext> _repository;

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

            AccountController.MD5Provider = new MD5CryptoServiceProvider();
        }

        public AccountController()
        {
            this._repository = new AccountRepository(AppGlobal.DbContext);
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
        public String GetVerifyCode(String bindingPhone)
        {
            return TaoBaoSms.SendVerifyCode(bindingPhone);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public String Login([FromBody]String encryptedJson)
        {
            String json = AccountController.RSAProvider.Decrypt(encryptedJson);
            LoginInfo loginInfo = JsonConvert.DeserializeObject<LoginInfo>(json);
            String token;
            if (IsTokenExsited(loginInfo.Name, loginInfo.Password, out token))
            {
                return token;
            }
            //验证用户
            Account account = this._repository.VerifyAccount(loginInfo.Name, loginInfo.Password);
            if (account != null)
            {
                return InitAccountToken(loginInfo.SecretKey, loginInfo.IV, account); ;
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RegisterSaler")]
        public String RegisterSaler([FromBody]String encryptedJson)
        {
            String json = AccountController.RSAProvider.Decrypt(encryptedJson);
            RegisterInfo registerInfo = JsonConvert.DeserializeObject<RegisterInfo>(json);
            if (!String.IsNullOrWhiteSpace(registerInfo.BindingPhone))
            {
                String token;
                if (IsTokenExsited(registerInfo.Name, registerInfo.Password, out token))
                {
                    return token;
                }
                Account account = null;
                if (TryRegisterAccount(registerInfo, "saler", out account))
                {
                    return InitAccountToken(registerInfo.SecretKey, registerInfo.IV, account); ;
                }
            }
            throw new HttpResponseException(HttpStatusCode.NotAcceptable);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RegisterBuyer")]
        public String RegisterBuyer([FromBody]String encryptedJson)
        {
            String json = AccountController.RSAProvider.Decrypt(encryptedJson);
            RegisterInfo registerInfo = JsonConvert.DeserializeObject<RegisterInfo>(json);
            if (!String.IsNullOrWhiteSpace(registerInfo.BindingPhone))
            {
                String token;
                if (IsTokenExsited(registerInfo.Name, registerInfo.Password, out token))
                {
                    return token;
                }
                Account account = null;
                if (TryRegisterAccount(registerInfo, "buyer", out account))
                {
                    return InitAccountToken(registerInfo.SecretKey, registerInfo.IV, account); ;
                }
            }
            throw new HttpResponseException(HttpStatusCode.NotAcceptable);
        }

        [RubbishRecycleAuthorize(Roles = "admin")]
        [RubbishRecycleAuthorize]
        [HttpGet]
        public IQueryable<Account> GetAllAccounts()
        {
            return this._repository.GetAllAccounts();
        }

        #endregion

        #region Private

        /// <summary>
        /// 注册用户信息。
        /// </summary>
        /// <param name="registerInfo">注册信息。</param>
        /// <param name="roleId">角色Id。</param>
        /// <param name="account">账户信息。</param>
        /// <returns>成功返回true；否则返回false。</returns>
        private Boolean TryRegisterAccount(RegisterInfo registerInfo, String roleId, out Account account)
        {
            account = new Account();
            account.RoleId = roleId;
            account.Id = Guid.NewGuid().GetHashCode();
            account.Name = String.IsNullOrWhiteSpace(registerInfo.Name) ? registerInfo.BindingPhone : registerInfo.Name;
            account.BindingPhone = registerInfo.BindingPhone;
            account.Password = CryptoHelper.MD5Compute(registerInfo.Password);
            account.LastLogin = DateTime.Now;
            account = this._repository.AddAccount(account);
            return account != null;
        }

        /// <summary>
        /// 初始化账户令牌。
        /// </summary>
        /// <param name="secretKey">客户端密钥。</param>
        /// <param name="iv">客户端加密向量。</param>
        /// <param name="account">账户信息。</param>
        /// <returns>登陆Token。</returns>
        private String InitAccountToken(Byte[] secretKey,Byte[] iv, Account account)
        {
            AccountToken accountToken = CreateAccountToken(secretKey, iv, account);
            AccountTokenManager.Manager.Add(accountToken);
            return accountToken.Token;
        }

        /// <summary>
        /// 创建账户的Token。
        /// </summary>
        /// <param name="secretKey">客户端密钥。</param>
        /// <param name="iv">客户端加密向量。</param>
        /// <param name="account">账户信息。</param>
        /// <returns>账户Token。</returns>
        private AccountToken CreateAccountToken(Byte[] secretKey,Byte[] iv, Account account)
        {
            try
            {
                //创建安全上下文
                ICryptoTransform decryptor = AccountController.AESProvider.CreateDecryptor(secretKey, iv);
                ICryptoTransform encryptor = AccountController.AESProvider.CreateEncryptor(secretKey, iv);
                AESCryptor cryptor = new AESCryptor(encryptor, decryptor);
                Int32 tokenMapKey = AccountToken.GenerateTokenMapKey(account.Name, account.Password);
                AccountToken accountToken = new AccountToken(account.Id, tokenMapKey, cryptor);
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

        /// <summary>
        /// 用户是否已经登陆。
        /// </summary>
        /// <param name="name">用户名。</param>
        /// <param name="password">密码。</param>
        /// <param name="token">登陆token。</param>
        /// <returns>如果已经登陆则返回true；否则返回false。</returns>
        private Boolean IsTokenExsited(String name, String password, out String token)
        {
            Int32 tokenMapKey = AccountToken.GenerateTokenMapKey(name, password);
            AccountToken accountToken = AccountTokenManager.Manager.GetTokenByMapKey(tokenMapKey);
            if (accountToken != null)
            {
                token = accountToken.Token;
                return true;
            }
            token = null;
            return false;
        }

        #endregion

        #region Misc

        /// <summary>
        /// 加密登陆响应消息。
        /// </summary>
        /// <param name="secretKey">创建Token使用的客户端密钥。</param>
        /// <param name="account">客户端账号信息。</param>
        /// <returns>加密消息。</returns>
        //private String EncryptLoginResponseMessage(Byte[] secretKey, Account account)
        //{
        //    LoginResult result = InitAccountToken(secretKey, account);
        //    String json = JsonConvert.SerializeObject(result);
        //    return AccountController.RSAProvider.Encrypt(json);
        //}

        #endregion

        #endregion
    }
}
