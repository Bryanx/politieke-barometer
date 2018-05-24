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
				//** NEEDED FOR TESTING WITH SUBPLATFORMS **//
				//int id = (filterContext.ActionParameters["id"] as Int32?).GetValueOrDefault();
				int id = 1;

				//bool partOfSubplatform = IsItemInSubPlatform(id, GetSubDomain(HttpContext.Current.Request.Url));
				bool partOfSubplatform =  true;

				if (!partOfSubplatform)
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

		/// <summary>
		/// Determines if the itemId is in the correct subpltform
		/// </summary>
		private static bool IsItemInSubPlatform(int itemID, int subPlatformId)
		{
			//Get subplatform
			SubPlatform subplatform = new SubplatformManager().GetSubPlatform(subPlatformId);
			if (subplatform == null) return false;

			//Get item
			Item item = new ItemManager().GetItemWithSubPlatform(itemID);
			if (item == null) return false;

			//Do check
			if(item.SubPlatform.SubPlatformId == subPlatformId) return true;	
			else return false;
		}

		/// <summary>
		/// Gives back the subdomain that you are currently on
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