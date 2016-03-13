using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RubbishRecycle.Main.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        #region Fields



        #endregion

        [AllowAnonymous]
        public void GetPublicKey()
        {
        }
    }
}
