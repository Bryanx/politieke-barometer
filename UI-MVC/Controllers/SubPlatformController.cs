using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BAR.UI.MVC.Controllers
{
  /// <summary>
  /// This controller is used to manage subplatforms
  /// </summary>
  public class SubPlatformController : LanguageController
  {
    // GET: SubPlatform
    public ActionResult Index()
    {
      return View();
    }
  }
}