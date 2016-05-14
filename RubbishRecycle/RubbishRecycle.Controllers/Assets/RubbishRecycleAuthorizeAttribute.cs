using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace RubbishRecycle.Controllers.Assets
{
    public class RubbishRecycleAuthorizeAttribute : AuthorizeAttribute
    {
        #region Constructors
        #endregion

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            AccountToken accountToken = actionContext.Request.GetAccountTokenByRequestHeader();
            if (accountToken != null)
            {
                IPrincipal principal = new BaseAccountTokenPrincipal(accountToken);
                if(principal.IsInRole(base.Roles))
                {
                    actionContext.RequestContext.Principal = new BaseAccountTokenPrincipal(accountToken);
                    IsAuthorized(actionContext);
                    return;
                }
            }
            HandleUnauthorizedRequest(actionContext);
        }

    }
}
