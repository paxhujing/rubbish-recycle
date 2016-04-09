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
            String str = RSAEncrypt(info, publicKey);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/account/RegisterBuyer");
            request.Content = new StringContent(str);
            String json = base.SendAsync(request).Result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<LoginResult>(json);
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

        #endregion

        #region Async

        public void RequestCommunicationAsync(Action<String> callback)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/account/RequestCommunication");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            base.SendAsync(request).ContinueWith((r) => callback(r.Result.Content.ReadAsStringAsync().Result));
        }

        public void RegisterBuyerAsync(RegisterInfo info, String publicKey, Action<LoginResult> callback)
        {
            String str = RSAEncrypt(info, publicKey);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/account/RegisterBuyer");
            request.Content = new StringContent(str);
            base.SendAsync(request).ContinueWith((r) =>
            {
                String json = r.Result.Content.ReadAsStringAsync().Result;
                LoginResult result = JsonConvert.DeserializeObject<LoginResult>(json);
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
