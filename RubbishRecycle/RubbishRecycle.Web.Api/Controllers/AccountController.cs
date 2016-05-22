using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace RubbishRecycle.Web.Api.Controllers
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
        [Route("GetPublicKey")]
        public HttpResponseMessage GetPublicKey()
        {
            HttpResponseMessage response = base.ActionContext.Request.CreateResponse (System.Net.HttpStatusCode.OK);
            response.Headers.Add("PublicKey", AccountController.GlobalPublicKey);
            return response;
        }

        #endregion
    }
}
