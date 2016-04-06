using RubbishRecycle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.PC.Contract
{
    public interface IAccountProxy
    {
        #region Sync

        String RequestCommunication();

        LoginResult RegisterBuyer(RegisterInfo info, String publicKey);

        #endregion

        #region Async

        void RequestCommunicationAsync(Action<Task<HttpResponseMessage>> callback);

        void RegisterBuyerAsync(RegisterInfo info, String publicKey, Action<Task<HttpResponseMessage>> callback);

        #endregion
    }
}
