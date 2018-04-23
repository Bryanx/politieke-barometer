using BAR.BL.Domain.Core;
using BAR.BL.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BAR.UI.MVC.Attributes
{
  public class SubPlatformCheckAPI : ActionFilterAttribute
  {
    public override void OnActionExecuting(HttpActionContext actionContext)
    {
      int subdomainID = GetSubDomain(HttpContext.Current.Request.Url);
      actionContext.Request.Properties.Add("SubPlatformID", subdomainID);

      base.OnActionExecuting(actionContext);
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
          if (subplatform == null)
          {
            return -1;
          }
          else
          {
            return subplatform.SubPlatformId;
          }
        }
      }
      return -1;
    }
  }
}