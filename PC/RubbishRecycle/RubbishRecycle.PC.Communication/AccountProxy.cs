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

        public OperationResult<String> RequestCommunication()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/account/RequestCommunication");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            HttpResponseMessage response = base.SendAsync(request).Result;
            OperationResult<String> publicKey = response.Content.ReadAsAsync<OperationResult<String>>().Result;
            return publicKey;
        }

        public OperationResult<String> RegisterBuyer(RegisterInfo info,String publicKey)
        {
            String str = RSAEncrypt(info, publicKey);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/account/RegisterBuyer");
            request.Content = new StringContent(str);
            return base.SendAsync(request).Result.Content.ReadAsAsync<OperationResult<String>>().Result;
        }

        public OperationResult<Account> GetAccount(String token, RijndaelManaged aesProvider)
        {
            //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/account/GetAccount");
            //request.Headers.Authorization = new AuthenticationHeaderValue("basic", token);
            //String json = base.SendAsync(request).Result.Content.ReadAsStringAsync().Result;
            //ICryptoTransform decryptor = aesProvider.CreateDecryptor();
            //AESCryptor cryptor = new AESCryptor(null, decryptor);
            //json = cryptor.Dencrypt(json);
            //return JsonConvert.DeserializeObject<Account>(json);
            throw new NotImplementedException();
        }

        public OperationResult<String> GetRegisterVerifyCode(String bindingPhone)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/account/GetRegisterVerifyCode?bindingPhone=" + bindingPhone);
            HttpResponseMessage response = base.SendAsync(request).Result;
            return response.Content.ReadAsAsync<OperationResult<String>>().Result;
        }

        #endregion

        #region Async

        public void RequestCommunicationAsync(Action<OperationResult<String>> callback)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/account/RequestCommunication");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            base.SendAsync(request).ContinueWith((r) => callback(r.Result.Content.ReadAsAsync<OperationResult<String>>().Result));
        }

        public void RegisterBuyerAsync(RegisterInfo info, String publicKey, Action<OperationResult<String>> callback)
        {
            String str = RSAEncrypt(info, publicKey);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/account/RegisterBuyer");
            request.Content = new StringContent(str);
            base.SendAsync(request).ContinueWith((r) =>
            {
                OperationResult<String> result = r.Result.Content.ReadAsAsync<OperationResult<String>>().Result;
                callback(result);
            });
        }

        #endregion

        #region Misc

        private String RSAEncrypt(Object obj,String publicKey)
        {
            String json = JsonConvert.SerializeObject(obj);
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(publicKey);
            return provider.Encrypt(json);
        }

        #endregion
    }
}
