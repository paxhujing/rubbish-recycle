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
            //String baseUrl = @"http://localhost:49811";
            String baseUrl = ConfigurationManager.AppSettings["baseUrl"];

            if (String.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException("baseUrl");
            }
            this.BaseAddress = new Uri(baseUrl);
        }

#endregion
    }
}
