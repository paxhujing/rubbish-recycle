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

        public String RegisterBuyer(RegisterInfo info,String publicKey)
        {
            String str = RSAEncrypt(info, publicKey);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/account/RegisterBuyer");
            request.Content = new StringContent(str);
            return base.SendAsync(request).Result.Content.ReadAsStringAsync().Result;
        }

        public Account GetAccount(String token, RijndaelManaged aesProvider)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/account/GetAccount");
            request.Headers.Authorization = new AuthenticationHeaderValue("basic", token);
            String json = base.SendAsync(request).Result.Content.ReadAsStringAsync().Result;
            ICryptoTransform decryptor = aesProvider.CreateDecryptor();
            AESCryptor cryptor = new AESCryptor(null, decryptor);
            json = cryptor.Dencrypt(json);
            return JsonConvert.DeserializeObject<Account>(json);
        }

        public String GetVerifyCode(String bindingPhone)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/account/GetVerifyCode?bindingPhone=" + bindingPhone);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            HttpResponseMessage response = base.SendAsync(request).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        #endregion

        #region Async

        public void RequestCommunicationAsync(Action<String> callback)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/account/RequestCommunication");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            base.SendAsync(request).ContinueWith((r) => callback(r.Result.Content.ReadAsStringAsync().Result));
        }

        public void RegisterBuyerAsync(RegisterInfo info, String publicKey, Action<String> callback)
        {
            String str = RSAEncrypt(info, publicKey);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/account/RegisterBuyer");
            request.Content = new StringContent(str);
            base.SendAsync(request).ContinueWith((r) =>
            {
                String token = r.Result.Content.ReadAsStringAsync().Result;
                callback(token);
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
