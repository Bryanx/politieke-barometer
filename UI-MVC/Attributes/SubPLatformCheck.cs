﻿using BAR.BL.Domain.Core;
using BAR.BL.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BAR.UI.MVC.Attributes
{
  public class SubPlatformCheck : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      int subdomainID = GetSubDomain(HttpContext.Current.Request.Url);

      base.OnActionExecuting(filterContext);

      if (subdomainID == -1)
      {
        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
        {
          controller = "Overview",
          action = "Index"
        }));
      } else
      {
        filterContext.RouteData.Values.Add("SubPlatformID", subdomainID);
      }
      
    }

    private static int GetSubDomain(Uri url)
    {
      string host = url.Host;
      if (host.Split('.').Length > 1)
      {
        int index = host.IndexOf(".");
        string subdomain = host.Substring(0, index);
        if (subdomain != "www")
        {
          ISubplatformManager subplatformManager = new SubplatformManager();
          SubPlatform subplatform = subplatformManager.GetSubPlatform(subdomain);
          if(subplatform == null)
          {
            return -1;
          } else
          {
            return subplatform.SubPlatformId;
          }
        }
      }
      return -1;
    }
  }
}