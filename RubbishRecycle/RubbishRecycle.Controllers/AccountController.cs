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
        public OperationResult<String> RequestCommunication()
        {
            return AppGlobal.GenerateSuccessResult<String>(AccountController.GlobalPublicKey);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("IsNameUsed")]
        public OperationResult<Boolean> IsNameUsed(String name)
        {
            Boolean isUsed = this._repository.IsNameUsed(name);
            OperationResult<Boolean> result = new OperationResult<Boolean>();
            result.Data = isUsed;
            return result;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("IsPhoneBinded")]
        public OperationResult<Boolean> IsPhoneBinded(String phone)
        {
            Boolean isBinded = this._repository.IsPhoneBinded(phone);
            OperationResult<Boolean> result = new OperationResult<Boolean>();
            result.Data = isBinded;
            return result;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetRegisterVerifyCode")]
        public OperationResult<String> GetRegisterVerifyCode(String bindingPhone)
        {
            String errorMessage;
            String code = TaoBaoSms.SendVerifyCode(bindingPhone,out errorMessage);
            return AppGlobal.GenerateResult<String>(code, errorMessage);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public OperationResult<String> Login([FromBody]String encryptedJson)
        {
            String json = AccountController.RSAProvider.Decrypt(encryptedJson);
            LoginInfo loginInfo = JsonConvert.DeserializeObject<LoginInfo>(json);
            Account account = this._repository.VerifyAccount(loginInfo.Name, loginInfo.Password);
            String token = null;
            if (account != null)
            {
                if (!IsTokenExsited(account, out token))
                {
                    token = InitAccountToken(loginInfo.SecretKey, loginInfo.IV, account);
                }
                return AppGlobal.GenerateSuccessResult<String>(token);
            }
            return AppGlobal.GenerateErrorResult<String>("账户不存在");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RegisterSaler")]
        public OperationResult<String> RegisterSaler([FromBody]String encryptedJson)
        {
            String json = AccountController.RSAProvider.Decrypt(encryptedJson);
            RegisterInfo registerInfo = JsonConvert.DeserializeObject<RegisterInfo>(json);
            String errorMessage;
            String token = Register(registerInfo, "saler", out errorMessage);
            return AppGlobal.GenerateResult<String>(token, errorMessage);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RegisterBuyer")]
        public OperationResult<String> RegisterBuyer([FromBody]String encryptedJson)
        {
            String json = AccountController.RSAProvider.Decrypt(encryptedJson);
            RegisterInfo registerInfo = JsonConvert.DeserializeObject<RegisterInfo>(json);
            String errorMessage;
            String token = Register(registerInfo, "buyer", out errorMessage);
            return AppGlobal.GenerateResult<String>(token, errorMessage);
        }

        [RubbishRecycleAuthorize(Roles = "admin")]
        [HttpGet]
        public IQueryable<Account> GetAllAccounts()
        {
            return this._repository.GetAllAccounts();
        }

        [RubbishRecycleAuthorize(Roles ="admin;saler;buyer")]
        [HttpGet]
        public Account GetAccount(String name)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private

        private String Register(RegisterInfo registerInfo, String roleId, out String errorMessage)
        {
            errorMessage = null;
            Account account = RegisterCore(registerInfo, roleId, out errorMessage);
            if (account != null)
            {
                return InitAccountToken(registerInfo.SecretKey, registerInfo.IV, account);
            }
            return null;
        }

        /// <summary>
        /// 注册用户信息。
        /// </summary>
        /// <param name="registerInfo">注册信息。</param>
        /// <param name="roleId">角色Id。</param>
        /// <param name="account">账户信息。</param>
        /// <returns>成功返回true；否则返回false。</returns>
        private Account RegisterCore(RegisterInfo registerInfo, String roleId, out String errorMessage)
        {
            errorMessage = null;
            if (String.IsNullOrWhiteSpace(registerInfo.BindingPhone))
            {
                errorMessage = "手机号不能为空";
                return null;
            }

            String verifyCode = VerifyCodeManager.Manager.GetCodeByPhone(registerInfo.BindingPhone);
            if (verifyCode != registerInfo.VerifyCode)
            {
                errorMessage = "验证码错误";
                return null;
            }
            String name = registerInfo.Name ?? registerInfo.BindingPhone;
            Account account = this._repository.FindAccount(name);
            if (account != null)
            {
                errorMessage = "此账户名已被使用，或者手机号已被绑定";
                return null;
            }
            account = new Account();
            account.RoleId = roleId;
            account.Id = Guid.NewGuid().GetHashCode();
            account.Name = name;
            account.BindingPhone = registerInfo.BindingPhone;
            account.Password = CryptoHelper.MD5Compute(registerInfo.Password);
            account.LastLogin = DateTime.Now;
            account = this._repository.AddAccount(account);
            return account;
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
                AccountToken accountToken = new AccountToken(account.Id, cryptor);
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
        private Boolean IsTokenExsited(Account account, out String token)
        {
            AccountToken accountToken = AccountTokenManager.Manager.GetTokenById(account.Id);
            if (accountToken != null)
            {
                token = accountToken.Token;
                return true;
            }
            token = null;
            return false;
        }

        #endregion

        #endregion
    }
}
