using BAR.BL.Domain.Core;
using BAR.BL.Managers;
using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BAR.UI.MVC.Attributes
{
	public class SubPlatformCheckAPI : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			//int subdomainId = GetSubDomain(HttpContext.Current.Request.Url);
			int subdomainId = 1;
			try
			{
				actionContext.Request.Properties.Add("SubPlatformID", subdomainId);
#pragma warning disable CS0168 // Variable is declared but never used
			}
			catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
			{
				//Do Nothing, already added
			}

			base.OnActionExecuting(actionContext);
		}

		/// <summary>
		/// Gives back the id of the current subplatform
		/// </summary>
		private static int GetSubDomain(Uri url)
		{
			string host = url.Host;
			if (host.Split('.').Length > 1)
			{
				int index = host.IndexOf(".");
				string subdomain = host.Substring(0, index);
				if (subdomain != "www")
				{
					SubPlatform subplatform = new SubplatformManager().GetSubPlatform(subdomain);
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
			return 1;
		}
	}
}