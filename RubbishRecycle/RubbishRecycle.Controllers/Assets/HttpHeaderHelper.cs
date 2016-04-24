using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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

        public static AccountToken GetTokenByRequestHeader(this HttpRequestMessage request)
        {
            String token = null;
            if (request.TryExtractToken(out token))
            {
                return AccountTokenManager.Manager.GetTokenByToken(token);
            }
            return null;
        }
    }
}
