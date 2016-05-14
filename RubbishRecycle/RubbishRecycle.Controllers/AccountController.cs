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
using RubbishRecycle.Models.ViewModels;

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

        #region Register misc

        [AllowAnonymous]
        [HttpPost]
        [ActionName("IsNameUsed")]
        public OperationResult IsNameUsed(RequestParamBeforeSignIn<String> info)
        {
            if (IsLegalRequest(info.AppKey))
            {
                Boolean isUsed = this._repository.IsNameUsed(info.Data);
                return OperationResultHelper.GenerateSuccessResult(isUsed);
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
                return OperationResultHelper.GenerateSuccessResult(isUsed);
            }
            return OperationResultHelper.GenerateErrorResult("无法识别的客户端");
        }

        #endregion

        #region Login Logout

        [AllowAnonymous]
        [HttpPost]
        [ActionName("Login")]
        public OperationResult Login(LoginInfo loginInfo)
        {
            if (IsLegalRequest(loginInfo.AppKey))
            {
                if(String.IsNullOrWhiteSpace(loginInfo.Name))
                {
                    return OperationResultHelper.GenerateErrorResult("账户不能为空");
                }
                String token = null;
                if(!AccountTokenManager.Manager.TryGetTokenByPhone(loginInfo.Name,out token))
                {
                    Account account = this._repository.VerifyAccount(loginInfo.Name, loginInfo.Password);
                    if (account != null)
                    {
                        token = InitAccountToken(account);
                    }
                    else
                    {
                        return OperationResultHelper.GenerateErrorResult("账户不存在或密码错误");
                    }
                }
                return OperationResultHelper.GenerateSuccessResult(token);
            }
            return OperationResultHelper.GenerateErrorResult("无法识别的客户端");
        }

        [RubbishRecycleAuthorize(Roles = "admin;saler;buyer")]
        [HttpGet]
        [ActionName("Logout")]
        public OperationResult Logout()
        {
            String token = base.ActionContext.GetToken();
            AccountTokenManager.Manager.Remove(token);
            return OperationResultHelper.GenerateSuccessResult();
        }

        #endregion

        #region Forget password

        [AllowAnonymous]
        [HttpPost]
        [ActionName("GetForgetPasswordVerifyCode")]
        public OperationResult GetForgetPasswordVerifyCode(RequestParamBeforeSignIn<String> info)
        {
            if (IsLegalRequest(info.AppKey))
            {
                if (this._repository.IsExisted(info.Data))
                {
                    return SendVerifyCode(info.Data, VerifyCodeType.ChangePassword);
                }
                return OperationResultHelper.GenerateErrorResult("账户不存在");
            }
            return OperationResultHelper.GenerateErrorResult("无法识别的客户端");
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("ForgetPassword")]
        public OperationResult ForgetPassword(ForgetPasswordInfo info)
        {
            if (IsLegalRequest(info.AppKey))
            {
                String verifyCode = VerifyCodeManager.Manager.GetCodeByPhone(info.Phone, VerifyCodeType.ChangePassword);
                if (verifyCode != info.VerifyCode)
                {
                    return OperationResultHelper.GenerateErrorResult("验证码错误");
                }
                if (this._repository.ChangePassword(info.Phone, info.Password))
                {
                    return OperationResultHelper.GenerateSuccessResult();
                }
                return OperationResultHelper.GenerateErrorResult("修改密码失败");
            }
            return OperationResultHelper.GenerateErrorResult("无法识别的客户端");
        }

        #endregion

        #region Register

        [AllowAnonymous]
        [HttpPost]
        [ActionName("GetRegisterVerifyCode")]
        public OperationResult GetRegisterVerifyCode(RequestParamBeforeSignIn<String> info)
        {
            if (IsLegalRequest(info.AppKey))
            {
                if (this._repository.IsExisted(info.Data))
                {
                    return OperationResultHelper.GenerateErrorResult("手机号已被绑定");
                }
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

        #endregion

        #region Change password

        [RubbishRecycleAuthorize(Roles = "admin;saler;buyer")]
        [HttpGet]
        [ActionName("GetChangePasswordVerifyCode")]
        public OperationResult GetChangePasswordVerifyCode()
        {
            String phone = base.ActionContext.GetPhone();
            return SendVerifyCode(phone, VerifyCodeType.ChangePassword);
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
            String phone = base.ActionContext.GetPhone();
            String verifyCode = VerifyCodeManager.Manager.GetCodeByPhone(phone, VerifyCodeType.ChangePassword);
            if (verifyCode != info.VerifyCode)
            {
                return OperationResultHelper.GenerateErrorResult("验证码错误");
            }
            if (this._repository.ChangePassword(phone, info.Password))
            {
                AccountTokenManager.Manager.Remove(base.ActionContext.GetToken());
                return OperationResultHelper.GenerateSuccessResult();
            }
            return OperationResultHelper.GenerateErrorResult("修改密码失败");
        }

        #endregion

        #region Account misc

        [RubbishRecycleAuthorize(Roles = "admin")]
        [HttpGet]
        public IQueryable<Account> GetAllAccounts()
        {
            return this._repository.GetAllAccounts();
        }

        [RubbishRecycleAuthorize(Roles ="admin;saler;buyer")]
        [HttpGet]
        [ActionName("GetAccountView")]
        public OperationResult GetAccountView()
        {
            String phone = base.ActionContext.GetPhone();
            AccountView view = this._repository.GetAccount(phone).ToViewer();
            return OperationResultHelper.GenerateSuccessResult(view);
        }

        #endregion

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

            String verifyCode = VerifyCodeManager.Manager.GetCodeByPhone(registerInfo.BindingPhone, VerifyCodeType.Register);
            if (verifyCode != registerInfo.VerifyCode)
            {
                errorMessage = "验证码错误";
                return null;
            }
            Account account = new Account();
            account.RoleId = roleId;
            account.Id = Guid.NewGuid().ToString().Replace("-",String.Empty);
            account.Name = registerInfo.Name ?? registerInfo.BindingPhone;
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
            AccountToken viewer = new AccountToken(account.BindingPhone);
            viewer.Role = account.RoleId;
            viewer.IsFreeze = account.IsFreezed;
            this._repository.UpdateLastLoginTime(account.Id);
            return AccountTokenManager.Manager.Add(viewer);
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
            String errorMessage = null;
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
                return OperationResultHelper.GenerateErrorResult(String.Format("发送验证码失败: {0}", errorMessage));
            }
            else
            {
                VerifyCodeManager.Manager.Add(phone, code, type);
            }
            return OperationResultHelper.GenerateSuccessResult();
        }

        #endregion

        #endregion
    }
}
