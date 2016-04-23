using Newtonsoft.Json;
using RubbishRecycle.Controllers.Assets;
using RubbishRecycle.Main.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

//[assembly: log4net.Config.XmlConfigurator(Watch = true, ConfigFile = "log4net.config")]
namespace RubbishRecycle.Main
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            GlobalConfiguration.Configuration.Formatters.Add(new TextPlainFormatter());

            //JsonMediaTypeFormatter jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            //jsonFormatter.Indent = true;
            //jsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            //jsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;

            //GlobalConfiguration.Configuration.Formatters.Clear();
            //GlobalConfiguration.Configuration.Formatters.Add(jsonFormatter);
        }
    }
}
