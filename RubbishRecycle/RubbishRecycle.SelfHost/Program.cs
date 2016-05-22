using RubbishRecycle.Web.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;

namespace RubbishRecycle.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpSelfHostConfiguration config = new HttpSelfHostConfiguration("http://localhost:8080");
            WebApiConfig.Register(config);
            HttpSelfHostServer host = new HttpSelfHostServer(config);
            host.OpenAsync().Wait();
            Console.WriteLine("Web api hosted at: {0}", config.BaseAddress);
            Console.ReadLine();
        }
    }
}
