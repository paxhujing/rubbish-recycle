using System;
using System.Collections.Concurrent;
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

        private static readonly ConcurrentDictionary<Int32, IEnumerable<String>> RolesCache;

        #endregion

        #region Constructors

        static RubbishRecycleAuthorizeAttribute()
        {
            RolesCache = new ConcurrentDictionary<Int32, IEnumerable<String>>();
        }

        #endregion

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            AccountToken accountToken = actionContext.Request.GetTokenByRequestHeader();
            if (accountToken != null)
            {
                if (String.IsNullOrWhiteSpace(base.Roles))
                {
                    IsAuthorized(actionContext);
                    return;
                }
                else
                {
                    Int32 hash = actionContext.ActionDescriptor.GetHashCode();
                    RolesCache.GetOrAdd(hash, base.Roles.Split(';'));
                    if (RolesCache[hash].Contains(accountToken.Role))
                    {
                        IsAuthorized(actionContext);
                        return;
                    }
                }
            }
            HandleUnauthorizedRequest(actionContext);
        }
    }
}
