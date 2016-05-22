using RubbishRecycle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.PC.Contract
{
    public interface IAccountProxy
    {
        #region Sync

        OperationResult RegisterBuyer(RegisterInfo info);

        OperationResult Login(LoginInfo info);

        OperationResult Logout(String token);

        OperationResult GetAccount(String token);

        OperationResult GetRegisterVerifyCode(RequestParamBeforeSignIn<String> requestParam);

        OperationResult GetChangePasswordVerifyCode(String token);

        OperationResult ChangePassword(ChangePasswordInfo info, String token);

        OperationResult GetForgetPasswordVerifyCode(RequestParamBeforeSignIn<String> info);

        OperationResult ForegetPassword(ForgetPasswordInfo info);

        #endregion

        #region Async

        void RegisterBuyerAsync(RegisterInfo info, Action<OperationResult> callback);

        #endregion
    }
}
