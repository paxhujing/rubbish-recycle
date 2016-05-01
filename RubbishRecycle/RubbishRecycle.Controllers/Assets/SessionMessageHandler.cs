using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubbishRecycle.Controllers.Assets
{
    public class SessionMessageHandler: DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                return Task<HttpResponseMessage>.FromResult(response);
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
