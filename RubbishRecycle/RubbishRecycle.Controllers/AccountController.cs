using RubbishRecycle.Controllers.Assets;
using RubbishRecycle.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Web.Http;
using System.Linq;
using System.Text;

namespace RubbishRecycle.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        #region Fields

        private static readonly RSACryptoServiceProvider RSAProvider;

        private static readonly String GlobalPrivateKey;

        private static readonly String GlobalPublicKey;

        #endregion

        #region Constructors

        static AccountController()
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            AccountController.RSAProvider = provider;
            AccountController.GlobalPrivateKey = provider.ToXmlString(true);
            AccountController.GlobalPublicKey = provider.ToXmlString(false);
        }

        public AccountController()
        {

        }

        #endregion

        #region Methods

        [AllowAnonymous]
        [HttpGet]
        [Route("RequestCommunication")]
        public HttpResponseMessage RequestCommunication()
        {
            HttpResponseMessage response = base.ActionContext.Request.CreateResponse (System.Net.HttpStatusCode.OK);
            //negotiatory-encryption:协商加密
            response.Headers.Add("NE", AccountController.GlobalPublicKey);
            return response;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SetClientSecretKey")]
        public HttpResponseMessage SetClientSecretKey(String account,String password)
        {
            System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
            String reasonPhrase = String.Empty;
            HttpRequestHeaders headers = base.ActionContext.Request.Headers;
            //negotiatory-encryption:协商加密
            if (headers.Contains("NE"))
            {
                headers.GetValues("NE");
                String secretKey = headers.GetValues("NE").First();
                Byte[] data = Convert.FromBase64String(secretKey);
                AccountController.RSAProvider.FromXmlString(AccountController.GlobalPrivateKey);
                try
                {
                    data = AccountController.RSAProvider.Decrypt(data, true);
                    secretKey = Encoding.UTF8.GetString(data);
                    AccountToken token = new AccountToken(secretKey)
                    {
                        LastRequest = DateTime.Now.AddSeconds(30)
                    };
                    AccountTokenManager.Manager.Add(token);
                }
                catch (CryptographicException)
                {
                    statusCode = System.Net.HttpStatusCode.NotAcceptable;
                    reasonPhrase = "Invalid base-64 string.";
                }
            }
            else
            {
                statusCode = System.Net.HttpStatusCode.BadRequest;
                reasonPhrase = "can not found key 'NE'.";
            }
            HttpResponseMessage response = new HttpResponseMessage(statusCode);
            response.ReasonPhrase = reasonPhrase;
            return response;
        }

        #endregion
    }
}
