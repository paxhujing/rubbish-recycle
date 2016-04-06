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
        #region Fields

        private static readonly Dictionary<Int32, IEnumerable<String>> RolesCache;

        #endregion

        #region Constructors

        static RubbishRecycleAuthorizeAttribute()
        {
            RolesCache = new Dictionary<Int32, IEnumerable<String>>();
        }

        #endregion

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            AuthenticationHeaderValue authenticationHeader = actionContext.Request.Headers.Authorization;
            if (authenticationHeader != null)
            {
                String token = authenticationHeader.Parameter;
                if (!String.IsNullOrWhiteSpace(token))
                {
                    AccountToken accountToken = AccountTokenManager.Manager[token];
                    if (accountToken != null)
                    {
                        if (String.IsNullOrWhiteSpace(base.Roles))
                        {
                            IsAuthorized(actionContext);
                            actionContext.Request.Properties.Add("Token", accountToken);
                            return;
                        }
                        else
                        {
                            Int32 hash = actionContext.ActionDescriptor.GetHashCode();
                            if (!RolesCache.ContainsKey(hash))
                            {
                                RolesCache.Add(hash, base.Roles.Split(';'));
                            }
                            if (RolesCache[hash].Contains(accountToken.Role))
                            {
                                IsAuthorized(actionContext);
                                actionContext.Request.Properties.Add("Token", accountToken);
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
