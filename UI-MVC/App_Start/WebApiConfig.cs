﻿using Microsoft.Owin.Security.OAuth;
using System.Web.Http;

namespace BAR.UI.MVC
{
  public class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      // Web API routes
      config.MapHttpAttributeRoutes();

      config.Routes.MapHttpRoute(
          "DefaultApi",
          "api/{controller}/{id}",
          defaults: new { id = System.Web.Http.RouteParameter.Optional }
      );
    }
  }
}