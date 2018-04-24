using AutoMapper;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BAR.UI.MVC.Controllers
{
  /// <summary>
  /// This controller is used to give the overview of all the subplatforms
  /// </summary>
  public class OverviewController : Controller
  {
    private const string INDEX_PAGE_TITLE = "Politieke Barometer";
    private IUserManager userManager;
    private ISubplatformManager subplatformManager;

    /// <summary>
		/// Overview page with all the subdomains, both for logged-in and non-logged-in users.
		/// </summary>
		[AllowAnonymous]
    public ActionResult Index()
    {
      userManager = new UserManager();
      subplatformManager = new SubplatformManager();

      //Assembling the view
      return View(new SubPlatformViewModel()
      {
        PageTitle = "Subplatform overview",
        User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
        SubPlatforms = Mapper.Map(subplatformManager.GetSubplatforms(), new List<SubPlatformDTO>())
      });
    }
  }
}