using System.Web.Mvc;
using BAR.UI.MVC.Controllers;

namespace BAR.UI.MVC.Helpers
{

	/// <summary>
	/// These helpers act as a table of contents of all URLs on the website.
	/// If a Controller/action is refactored it should be changed here.
	/// </summary>
	public static class UrlExtensions
	{
		public static string RootUrl(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "Home",
				action = nameof(HomeController.Index)
			});
		}

		public static string NewsUrl(this UrlHelper helper)
		{
			return RootUrl(helper) + "#nieuws";
		}

		public static string PrivacyUrl(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "Home",
				action = nameof(HomeController.Privacy)
			});
		}

		public static string FaqUrl(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "Home",
				action = nameof(HomeController.Faq)
			});
		}

		#region UserUrls

		public static string DashboardUrl(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "User",
				action = nameof(UserController.Index)
			});
		}

		public static string UserWeeklyReview(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "User",
				action = nameof(UserController.UserWeeklyReview)
			});
		}

		public static string SettingsUrl(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "User",
				action = nameof(UserController.Settings)
			});
		}

		public static string ForgotPasswordUrl(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "User",
				action = nameof(UserController.ForgotPassword)
			});
		}

		public static string ProfilePictureUrl(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "User",
				action = nameof(UserController.ProfilePicture)
			});
		}
		
		public static string ProfilePictureUrlForUser(this UrlHelper helper, string userId)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "User",
				action = nameof(UserController.ProfilePictureForUser),
				id = userId
			});
		}

		public static string HeaderImage(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "Home",
				action = nameof(HomeController.HeaderImage)
			});
		}

		public static string LogoImage(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "Home",
				action = nameof(HomeController.LogoImage)
			});
		}

		public static string DarkLogoImage(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "Home",
				action = nameof(HomeController.DarkLogoImage)
			});
		}

		#endregion

		#region ItemUrls

		public static string PersonUrl(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "Person",
				action = nameof(PersonController.Index)
			});
		}

		public static string PersonUrl(this UrlHelper helper, int personId)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "Person",
				action = nameof(PersonController.Details),
				id = personId
			});
		}

		public static string ThemeUrl(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "Theme",
				action = nameof(ThemeController.Index)
			});
		}

		public static string ThemeUrl(this UrlHelper helper, int personId)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "Theme",
				action = nameof(ThemeController.Details),
				id = personId
			});
		}

		public static string OrganisationUrl(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "Organisation",
				action = nameof(OrganisationController.Index)
			});
		}

		public static string OrganisationUrl(this UrlHelper helper, int organisationId)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "Organisation",
				action = nameof(OrganisationController.Details),
				id = organisationId
			});
		}
		public static string ItemPictureUrl(this UrlHelper helper, int itemId)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "Person",
				action = nameof(PersonController.Picture),
				id = itemId
			});
		}
		#endregion

		#region AdminUrls

		public static string AdminDashboard(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "Admin",
				action = nameof(AdminController.Index)
			});
		}

		public static string PageManagement(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "Admin",
				action = nameof(AdminController.PageManagement)
			});
		}

		public static string ItemManagement(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "Admin",
				action = nameof(AdminController.ItemManagement)
			});
		}

		public static string UserManagement(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "Admin",
				action = nameof(AdminController.UserManagement)
			});
		}

		#endregion

		#region SuperAdminUrls

		public static string SourceManagement(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "SuperAdmin",
				action = nameof(SuperAdminController.GeneralManagement)
			});
		}

		public static string PlatformManagement(this UrlHelper helper)
		{
			return helper.RouteUrl("Default", new
			{
				controller = "SuperAdmin",
				action = nameof(SuperAdminController.PlatformManagement)
			});
		}

		#endregion
	}
}