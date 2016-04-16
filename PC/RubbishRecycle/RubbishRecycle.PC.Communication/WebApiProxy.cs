using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.PC.Communication
{
    public class WebApiProxy : HttpClient
    {
        #region Constructors

        public WebApiProxy()
        {
#if DEBUG
            String baseUrl = @"http://localhost:49811";
#else
            String baseUrl = ConfigurationManager.AppSettings["baseUrl"];
#endif
            if (String.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException("baseUrl");
            }
            this.BaseAddress = new Uri(baseUrl);
        }

#endregion
    }
}
