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
  public class ErrorController : Controller
  {
    private IUserManager userManager;
    public ViewResult Index()
    {
      userManager = new UserManager();
      return View(new BaseViewModel()
      {
        PageTitle = "Test index",
        User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null
      });
    }
    public ViewResult NotFound()
    {
      userManager = new UserManager();
      Response.StatusCode = 404;
      return View(new BaseViewModel()
      {
        PageTitle = "Test",
        User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null
      });
    }
  }
}