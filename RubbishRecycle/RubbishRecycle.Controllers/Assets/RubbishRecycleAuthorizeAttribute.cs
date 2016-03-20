using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace RubbishRecycle.Controllers.Assets
{
    public class RubbishRecycleAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            AuthenticationHeaderValue authenticationHeader = actionContext.Request.Headers.Authorization;
            if (authenticationHeader != null)
            {
                String token = authenticationHeader.Parameter;
                if (!String.IsNullOrEmpty(token))
                {
                    AccountToken accountToken = AccountTokenManager.Manager[token];
                    if (accountToken != null)
                    {
                        actionContext.Request.Properties.Add("AccountToken", accountToken);
                        if (String.IsNullOrEmpty(base.Roles))
                        {
                            IsAuthorized(actionContext);
                            return;
                        }
                        else
                        {
                            String[] roles = base.Roles.Split(',');
                            IEnumerable<String> existedRoles = accountToken.Roles.Intersect(roles, StringComparer.OrdinalIgnoreCase);
                            if (existedRoles.Count() != 0)
                            {
                                IsAuthorized(actionContext);
                                return;
                            }
                        }
                    }
                }
            }
            HandleUnauthorizedRequest(actionContext);
        }
    }
}
