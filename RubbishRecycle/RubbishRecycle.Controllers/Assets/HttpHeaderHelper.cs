using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace RubbishRecycle.Controllers.Assets
{
    internal static class HttpHeaderHelper
    {
        public static Boolean TryExtractToken(this HttpRequestMessage request,out String token)
        {
            token = null;
            AuthenticationHeaderValue authenticationHeader = request.Headers.Authorization;
            if (authenticationHeader != null)
            {
                token = authenticationHeader.Parameter;
            }
            return !String.IsNullOrWhiteSpace(token);
        }

        public static AccountToken GetAccountTokenByRequestHeader(this HttpRequestMessage request)
        {
            String token = null;
            if (request.TryExtractToken(out token))
            {
                return AccountTokenManager.Manager.GetAccountTokenByToken(token);
            }
            return null;
        }

        public static AccountToken GetAccountTokenFromActionContext(this HttpActionContext actionContext)
        {
            return (AccountToken)actionContext.ActionDescriptor.Properties["AccountToken"];
        }
    }
}
