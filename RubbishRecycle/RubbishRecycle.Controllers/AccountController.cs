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
        [HttpPost]
        [ActionName("IsNameUsed")]
        public OperationResult<Boolean> IsNameUsed(RequestParamBeforeSignIn<String> info)
        {
            if (IsLegalRequest(info.AppKey))
            {
                Boolean isUsed = this._repository.IsNameUsed(info.Data);
                OperationResult<Boolean> result = new OperationResult<Boolean>();
                result.Data = isUsed;
                return result;
            }
            return OperationResultHelper.GenerateErrorResult<Boolean>("无法识别的客户端");
        }

        [AllowAnonymous]
        [HttpPost]
        public OperationResult<Boolean> IsPhoneBinded(RequestParamBeforeSignIn<String> info)
        {
            if (IsLegalRequest(info.AppKey))
            {
                Boolean isUsed = this._repository.IsPhoneBinded(info.Data);
                OperationResult<Boolean> result = new OperationResult<Boolean>();
                result.Data = isUsed;
                return result;
            }
            return OperationResultHelper.GenerateErrorResult<Boolean>("无法识别的客户端");
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("GetRegisterVerifyCode")]
        public OperationResult GetRegisterVerifyCode(RequestParamBeforeSignIn<String> info)
        {
            if (IsLegalRequest(info.AppKey))
            {
                String errorMessage;
                String code = TaoBaoSms.SendVerifyCode(info.Data, out errorMessage);
                return OperationResultHelper.GenerateSuccessResult();
            }
            return OperationResultHelper.GenerateErrorResult("无法识别的客户端");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public OperationResult<String> Login(LoginInfo loginInfo)
        {
            if (IsLegalRequest(loginInfo.AppKey))
            {
                Account account = this._repository.VerifyAccount(loginInfo.Name, loginInfo.Password);
                String token = null;
                if (account != null)
                {
                    if (!IsTokenExsited(account, out token))
                    {
                        token = InitAccountToken(account);
                    }
                    return OperationResultHelper.GenerateSuccessResult<String>(token);
                }
                return OperationResultHelper.GenerateErrorResult<String>("账户不存在");
            }
            return OperationResultHelper.GenerateErrorResult<String>("无法识别的客户端");
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("RegisterSaler")]
        public OperationResult<String> RegisterSaler(RegisterInfo registerInfo)
        {
            return Register("saler", registerInfo);
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("RegisterBuyer")]
        public OperationResult<String> RegisterBuyer(RegisterInfo registerInfo)
        {
            return Register("buyer", registerInfo);
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

        public OperationResult<String> Register(String roleId, RegisterInfo registerInfo)
        {
            if (IsLegalRequest(registerInfo.AppKey))
            {
                String errorMessage;
                String token = RegisterAndInitToken(registerInfo, roleId, out errorMessage);
                return OperationResultHelper.GenerateResult<String>(token, errorMessage);
            }
            return OperationResultHelper.GenerateErrorResult<String>("无法识别的客户端");
        }

        /// <summary>
        /// 注册。如果注册成功则初始化账户的Token。
        /// </summary>
        /// <param name="registerInfo"></param>
        /// <param name="roleId"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private String RegisterAndInitToken(RegisterInfo registerInfo, String roleId, out String errorMessage)
        {
            errorMessage = null;
            Account account = RegisterCore(registerInfo, roleId, out errorMessage);
            if (account != null)
            {
                return InitAccountToken(account);
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
        /// <param name="account">账户信息。</param>
        /// <returns>登陆Token。</returns>
        private String InitAccountToken(Account account)
        {
            AccountToken accountToken = new AccountToken(account.Id);
            accountToken.Role = account.RoleId;
            AccountTokenManager.Manager.Add(accountToken);
            return accountToken.Token;
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
                token = accountToken.GenerateToken();
                return true;
            }
            token = null;
            return false;
        }

        /// <summary>
        /// 是否是合法的请求。
        /// </summary>
        /// <param name="appKey">AppKey。</param>
        /// <returns>合法返回true；否则返回false。</returns>
        private Boolean IsLegalRequest(String appKey)
        {
            return this._repository.GetAppKeyInfo(appKey) != null;
        }

        #endregion

        #endregion
    }
}
