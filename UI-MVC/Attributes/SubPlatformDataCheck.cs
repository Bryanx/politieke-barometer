using BAR.BL.Domain.Core;
using BAR.BL.Domain.Items;
using BAR.BL.Managers;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BAR.UI.MVC.Attributes
{
	public class SubPlatformDataCheck : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext.ActionParameters.ContainsKey("id"))
			{
				int id = (filterContext.ActionParameters["id"] as Int32?).GetValueOrDefault();

				bool partOfSubplatform = IsItemInSubPlatform(id, GetSubDomain(HttpContext.Current.Request.Url));

				if(!partOfSubplatform)
				{
					filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
					{
						controller = "Person",
						action = "Index"
					}));
				}
			}

			base.OnActionExecuting(filterContext);
		}

		private static bool IsItemInSubPlatform(int itemID, int subPlatformId)
		{
			ISubplatformManager subplatformManager = new SubplatformManager();
			SubPlatform subplatform = subplatformManager.GetSubPlatform(subPlatformId);

			IItemManager itemManager = new ItemManager();
			Item item = itemManager.GetItemWithSubPlatform(itemID);

			if(item.SubPlatform.SubPlatformId == subPlatformId)
			{
				return true;
			}
			return false;
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
			return 1;
		}
	}
}