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

        OperationResult<String> RequestCommunication();

        OperationResult<String> RegisterBuyer(RegisterInfo info, String publicKey);

        OperationResult<Account> GetAccount(String token, RijndaelManaged aesProvider);

        OperationResult<String> GetRegisterVerifyCode(RequestParamBeforeSignIn<String> requestParam, String publicKey);

        #endregion

        #region Async

        void RequestCommunicationAsync(Action<OperationResult<String>> callback);

        void RegisterBuyerAsync(RegisterInfo info, String publicKey, Action<OperationResult<String>> callback);

        #endregion
    }
}
