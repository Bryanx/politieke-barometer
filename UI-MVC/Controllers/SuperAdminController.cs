using System.Web.Mvc;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System;

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

    public ActionResult SynchronizeData()
    {
      using (HttpClient client = new HttpClient())
      {
        //Make request
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://kdg.textgain.com/query");
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("X-API-Key", "aEN3K6VJPEoh3sMp9ZVA73kkr");

        request.Content = new StringContent("{\"name\":\"Annick De Ridder\"}", Encoding.UTF8, "application/json");

        //Send request
        HttpResponseMessage response = client.SendAsync(request).Result;

        //Read response
        if (response.IsSuccessStatusCode)
        {
          var json = response.Content.ReadAsStringAsync().Result;
          IDataManager dataManager = new DataManager(new UnitOfWorkManager());
          dataManager.SynchronizeData(json);
        }
        else throw new Exception("Error: " + response.StatusCode);
      }
      return null;
    }
  }
}