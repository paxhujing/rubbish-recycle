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
            return base.SendAsync(request, cancellationToken).ContinueWith((r) =>
            {
                HttpResponseMessage rm = r.Result;
                if (rm.IsSuccessStatusCode)
                {
                    ObjectContent oc = rm.Content as ObjectContent;
                    if (oc != null)
                    {
                        AccountToken accountToken = request.GetTokenByRequestHeader();
                        String json = JsonConvert.SerializeObject(oc.Value);
                        String encryptContent = accountToken.Cryptor.Encrypt(json);
                        return new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            RequestMessage = rm.RequestMessage,
                            Content = new StringContent(encryptContent),
                            ReasonPhrase = rm.ReasonPhrase,
                        };
                    }
                }
                return rm;
            }); ;
        }
    }
}
