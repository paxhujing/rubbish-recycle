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

        public OperationResult RegisterBuyer(RegisterInfo info)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/account/RegisterBuyer");
            request.Content = new ObjectContent<RegisterInfo>(info, WebApiProxy.JsonFormatter);
            return base.SendAsync(request).Result.Content.ReadAsAsync<OperationResult>().Result;
        }

        public OperationResult Login(LoginInfo info)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/account/Login");
            request.Content = new ObjectContent<LoginInfo>(info, WebApiProxy.JsonFormatter);
            return base.SendAsync(request).Result.Content.ReadAsAsync<OperationResult>().Result;
        }

        public OperationResult Logout(String token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/account/Logout?token=" + token);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);
            return base.SendAsync(request).Result.Content.ReadAsAsync<OperationResult>().Result;
        }

        public OperationResult GetAccount(String token)
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

        public OperationResult GetRegisterVerifyCode(RequestParamBeforeSignIn<String> requestParam)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/account/GetRegisterVerifyCode");
            request.Content = new ObjectContent<RequestParamBeforeSignIn<String>>(requestParam, WebApiProxy.JsonFormatter);
            HttpResponseMessage response = base.SendAsync(request).Result;
            return response.Content.ReadAsAsync<OperationResult>().Result;
        }

        public OperationResult GetChangePasswordVerifyCode(String token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/account/GetChangePasswordVerifyCode");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);
            return base.SendAsync(request).Result.Content.ReadAsAsync<OperationResult>().Result;
        }

        public OperationResult ChangePassword(ChangePasswordInfo info, String token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/account/ChangePassword");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);
            request.Content = new ObjectContent<ChangePasswordInfo>(info, WebApiProxy.JsonFormatter);
            return base.SendAsync(request).Result.Content.ReadAsAsync<OperationResult>().Result;
        }

        public OperationResult GetForgetPasswordVerifyCode(RequestParamBeforeSignIn<String> info)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/account/GetForgetPasswordVerifyCode");
            request.Content = new ObjectContent<RequestParamBeforeSignIn<String>>(info, WebApiProxy.JsonFormatter);
            return base.SendAsync(request).Result.Content.ReadAsAsync<OperationResult>().Result;
        }

        public OperationResult ForegetPassword(ForgetPasswordInfo info)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/account/ForgetPassword");
            request.Content = new ObjectContent<ForgetPasswordInfo>(info, WebApiProxy.JsonFormatter);
            return base.SendAsync(request).Result.Content.ReadAsAsync<OperationResult>().Result;
        }

        #endregion

        #region Async

        public void RegisterBuyerAsync(RegisterInfo info, Action<OperationResult> callback)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/account/RegisterBuyer");
            request.Content = new ObjectContent<RegisterInfo>(info, WebApiProxy.JsonFormatter);
            base.SendAsync(request).ContinueWith((r) =>
            {
                OperationResult result = r.Result.Content.ReadAsAsync<OperationResult>().Result;
                callback(result);
            });
        }

        #endregion
    }
}
