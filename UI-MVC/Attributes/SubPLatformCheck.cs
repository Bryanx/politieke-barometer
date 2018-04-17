using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BAR.UI.MVC.Attributes
{
  public class SubPlatformCheck : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      string subdomain = GetSubDomain(HttpContext.Current.Request.Url);
    }

    private static string GetSubDomain(Uri url)
    {
      string host = url.Host;
      if (host.Split('.').Length > 1)
      {
        int index = host.IndexOf(".");
        string subdomain = host.Substring(0, index);
        if (subdomain != "www")
        {
          return subdomain;
        }
      }
      return null;
    }
  }
}