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
        public OperationResult IsNameUsed(RequestParamBeforeSignIn<String> info)
        {
            if (IsLegalRequest(info.AppKey))
            {
                Boolean isUsed = this._repository.IsNameUsed(info.Data);
                return OperationResultHelper.GenerateSuccessResult(isUsed.ToString());
            }
            return OperationResultHelper.GenerateErrorResult("无法识别的客户端");
        }

        [AllowAnonymous]
        [HttpPost]
        public OperationResult IsPhoneBinded(RequestParamBeforeSignIn<String> info)
        {
            if (IsLegalRequest(info.AppKey))
            {
                Boolean isUsed = this._repository.IsPhoneBinded(info.Data);
                return OperationResultHelper.GenerateSuccessResult(isUsed.ToString());
            }
            return OperationResultHelper.GenerateErrorResult("无法识别的客户端");
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("Login")]
        public OperationResult Login(LoginInfo loginInfo)
        {
            if (IsLegalRequest(loginInfo.AppKey))
            {
                Account account = this._repository.VerifyAccount(loginInfo.Name, loginInfo.Password);
                if (account != null)
                {
                    DropIfAccountTokenExsited(account);
                    String token = InitAccountToken(account);
                    return OperationResultHelper.GenerateSuccessResult(token);
                }
                return OperationResultHelper.GenerateErrorResult("账户不存在");
            }
            return OperationResultHelper.GenerateErrorResult("无法识别的客户端");
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("GetRegisterVerifyCode")]
        public OperationResult GetRegisterVerifyCode(RequestParamBeforeSignIn<String> info)
        {
            if (IsLegalRequest(info.AppKey))
            {
                return SendVerifyCode(info.Data, VerifyCodeType.Register);
            }
            return OperationResultHelper.GenerateErrorResult("无法识别的客户端");
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("RegisterSaler")]
        public OperationResult RegisterSaler(RegisterInfo registerInfo)
        {
            return Register("saler", registerInfo);
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("RegisterBuyer")]
        public OperationResult RegisterBuyer(RegisterInfo registerInfo)
        {
            return Register("buyer", registerInfo);
        }

        [RubbishRecycleAuthorize(Roles = "admin;saler;buyer")]
        [HttpGet]
        [ActionName("Logout")]
        public OperationResult Logout()
        {
            AccountToken at = base.ActionContext.GetAccountTokenFromActionContext();
            AccountTokenManager.Manager.Remove(at.Token);
            return OperationResultHelper.GenerateSuccessResult();
        }

        [RubbishRecycleAuthorize(Roles = "admin;saler;buyer")]
        [HttpGet]
        [ActionName("GetChangePasswordVerifyCode")]
        public OperationResult GetChangePasswordVerifyCode()
        {
            AccountToken at = base.ActionContext.GetAccountTokenFromActionContext();
            return SendVerifyCode(at.Phone, VerifyCodeType.ChangePassword);
        }

        [RubbishRecycleAuthorize(Roles = "admin;saler;buyer")]
        [HttpPost]
        [ActionName("ChangePassword")]
        public OperationResult ChangePassword(ChangePasswordInfo info)
        {
            if (String.IsNullOrWhiteSpace(info.Password))
            {
                return OperationResultHelper.GenerateErrorResult("密码不能为空");
            }
            AccountToken at = base.ActionContext.GetAccountTokenFromActionContext();
            String verifyCode = VerifyCodeManager.Manager.GetCodeByPhone(at.Phone);
            if (verifyCode != info.VerifyCode)
            {
                return OperationResultHelper.GenerateErrorResult("验证码错误");
            }
            if (this._repository.ChangePassword(at.Phone, info.Password))
            {
                return OperationResultHelper.GenerateSuccessResult();
            }
            return OperationResultHelper.GenerateErrorResult("修改密码失败");
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
            return this._repository.FindAccount(name);
        }

        #endregion

        #region Private

        private OperationResult Register(String roleId, RegisterInfo registerInfo)
        {
            if (IsLegalRequest(registerInfo.AppKey))
            {
                String errorMessage;
                String token = RegisterAndInitToken(registerInfo, roleId, out errorMessage);
                return OperationResultHelper.GenerateResult(token, errorMessage);
            }
            return OperationResultHelper.GenerateErrorResult("无法识别的客户端");
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
            account.Id = Guid.NewGuid().ToString().Replace("-",String.Empty);
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
            AccountToken accountToken = new AccountToken(account.BindingPhone);
            accountToken.Role = account.RoleId;
            AccountTokenManager.Manager.Add(accountToken);
            return accountToken.Token;
        }

        /// <summary>
        /// 如果AccountToken已经存在则删除。
        /// </summary>
        /// <param name="account">账户信息。</param>
        private void DropIfAccountTokenExsited(Account account)
        {
            AccountToken accountToken = AccountTokenManager.Manager.GetAccountTokenById(account.BindingPhone);
            if (accountToken != null)
            {
                AccountTokenManager.Manager.Remove(accountToken);
                AppGlobal.Log.DebugFormat("Drop existed token: {0}", accountToken.Token);
            }
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

        /// <summary>
        /// 发送验证码。
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="type">验证码类型。</param>
        /// <returns></returns>
        private OperationResult SendVerifyCode(String phone, VerifyCodeType type)
        {
            String errorMessage;
            String code = null;
            switch (type)
            {
                case VerifyCodeType.Register:
                    code = TaoBaoSms.SendRegisterVerifyCode(phone, out errorMessage);
                    break;
                case VerifyCodeType.ChangePassword:
                    code = TaoBaoSms.SendChangePasswordVerifyCode(phone, out errorMessage);
                    break;
                default:
                    break;
            }
            if (String.IsNullOrWhiteSpace(code))
            {
                return OperationResultHelper.GenerateErrorResult("发送给验证码失败");
            }
            else
            {
                VerifyCodeManager.Manager.Add(phone, code, VerifyCodeType.Register);
            }
            return OperationResultHelper.GenerateSuccessResult();
        }

        #endregion

        #endregion
    }
}
