using RubbishRecycle.PC.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.PC.Communication
{
    public class AccountProxy : WebApiProxy, IAccountProxy
    {
        #region Sync Methods

        public String RequestCommunication()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/account/RequestCommunication");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            HttpResponseMessage response = base.SendAsync(request).Result;
            String publicKey = response.Content.ReadAsStringAsync().Result;
            return publicKey;
        }

        #endregion

        #region Async

        public Task<HttpResponseMessage> RequestCommunicationAsync()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/account/RequestCommunication");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            return base.SendAsync(request);
        }
        
        #endregion
    }
}
