using System.Web.Mvc;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BAR.UI.MVC.Controllers
{

  [Authorize(Roles = "SuperAdmin")]
  public class SuperAdminController : Controller
  {

    IUserManager userManager = new UserManager();

    public ActionResult SourceManagement()
    {
      const string PAGE_TITLE = "Bronnen beheren";
      return View(new BaseViewModel
      {
        PageTitle = PAGE_TITLE,
        User = userManager.GetUser(User.Identity.GetUserId())
      });
    }

    public ActionResult AdminManagement()
    {
      const string PAGE_TITLE = "Admins beheren";
      return View(new BaseViewModel
      {
        PageTitle = PAGE_TITLE,
        User = userManager.GetUser(User.Identity.GetUserId())
      });
    }

    public ActionResult PlatformManagement()
    {
      const string PAGE_TITLE = "Deelplatformen beheren";
      return View(new BaseViewModel
      {
        PageTitle = PAGE_TITLE,
        User = userManager.GetUser(User.Identity.GetUserId())
      });
    }

    public async Task<ActionResult> SynchronizeData()
    {
      HttpClient client = new HttpClient();
      var values = new Dictionary<string, string>
        {
          { "name", "Geert Bourgeois" }
        };
      var content = new FormUrlEncodedContent(values);
      client.DefaultRequestHeaders.TryAddWithoutValidation("X-API-Key", "aEN3K6VJPEoh3sMp9ZVA73kkr");
      client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
      client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json; charset=utf-8");
      var response = await client.PostAsync("http://kdg.textgain.com/query", content);

      var responseString = await response.Content.ReadAsStringAsync();
      return null;
    }
  }
}