﻿using RubbishRecycle.Controllers.Assets;
using RubbishRecycle.Main.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace RubbishRecycle.Main
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Session",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: null
            );
            config.MessageHandlers.Add(new SessionMessageHandler());
            config.Filters.Add(new UnhandleExceptionFilterAttribute());
        }
    }
}
