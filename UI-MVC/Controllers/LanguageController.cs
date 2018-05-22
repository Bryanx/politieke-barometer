using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using BAR.BL.Domain.Core;
using BAR.BL.Managers;
using BAR.UI.MVC.Helpers;
using BAR.UI.MVC.Attributes;
using BAR.UI.MVC.Models;
using AutoMapper;
using System.Collections.Generic;

namespace BAR.UI.MVC.Controllers
{
	/// <summary>
	/// This controller stores the user preferred language in a cookie.
	/// </summary>
	[SubPlatformCheck]
	public class LanguageController : Controller
	{

		//storing the language defaults in a cookie
		protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
		{
			string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0
                    ? Request.UserLanguages[0]
                    : // obtain it from HTTP header AcceptLanguages
                    null;
            // Validate culture name
            cultureName = LanguageHelper.GetImplementedCulture(cultureName); // This is safe

			// Modify current thread's cultures            
			Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
			Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

			return base.BeginExecuteCore(callback, state);
		}

		//Customization options: Aliases, colors, etc.
		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);

			if (filterContext.Result is ViewResultBase result)
			{
				if (filterContext.Controller.ViewData.Model is BaseViewModel model)
				{
					SubplatformManager platformManager = new SubplatformManager();
					int subPlatformID = (int)RouteData.Values["SubPlatformID"];
					Customization customization = platformManager.GetCustomization(subPlatformID);
					model.Customization = Mapper.Map(customization, new CustomizationViewModel());
					model.Customization.Questions = Mapper.Map(platformManager.GetQuestions(subPlatformID), new List<QuestionDTO>());
				}
			}
		}
	}
}