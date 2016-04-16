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

        String RequestCommunication();

        String RegisterBuyer(RegisterInfo info, String publicKey);

        Account GetAccount(String token, RijndaelManaged aesProvider);

        VerifyCodeSmsResult GetVerifyCode(VerifyCodeRequest codeReuqest, String publicKey);

        #endregion

        #region Async

        void RequestCommunicationAsync(Action<String> callback);

        void RegisterBuyerAsync(RegisterInfo info, String publicKey, Action<String> callback);

        #endregion
    }
}
