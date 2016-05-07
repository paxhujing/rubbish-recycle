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

        OperationResult<String> RegisterBuyer(RegisterInfo info);

        OperationResult<Account> GetAccount(String token);

        OperationResult GetRegisterVerifyCode(RequestParamBeforeSignIn<String> requestParam);

        #endregion

        #region Async

        void RegisterBuyerAsync(RegisterInfo info, Action<OperationResult<String>> callback);

        #endregion
    }
}
