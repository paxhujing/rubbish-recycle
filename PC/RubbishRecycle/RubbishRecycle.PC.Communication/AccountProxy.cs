using RubbishRecycle.PC.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using RubbishRecycle.Models;
using System.Security.Cryptography;
using Newtonsoft.Json;
using RubbishRecycle.Toolkit;

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

        public LoginResult RegisterBuyer(RegisterInfo info,String publicKey)
        {
            String str = RSADecrypt(info, publicKey);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/account/RegisterBuyer");
            request.Content = new StringContent(str);
            return base.SendAsync(request).Result.Content.ReadAsAsync<LoginResult>().Result;
        }

        #endregion

        #region Async

        public void RequestCommunicationAsync(Action<Task<HttpResponseMessage>> callback)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/account/RequestCommunication");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            base.SendAsync(request).ContinueWith(callback);
        }

        public void RegisterBuyerAsync(RegisterInfo info, String publicKey, Action<Task<HttpResponseMessage>> callback)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Misc

        private String RSADecrypt(Object obj,String publicKey)
        {
            String json = JsonConvert.SerializeObject(obj);
            Byte[] data = Encoding.UTF8.GetBytes(json);

            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(publicKey);
            String temp = provider.Encrypt(data);
            return temp;
        }

        #endregion
    }
}
