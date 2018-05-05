using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using webapi.Providers;
using webapi.Results;
using BAR.UI.MVC.Models;
using BAR.BL.Managers;
using BAR.UI.MVC;
using BAR.BL.Domain.Users;
using BAR.BL.Domain.Widgets;
using System.Web.Script.Serialization;
using System.Linq;

namespace webapi.Controllers
{
  [Authorize]
  [RoutePrefix("api/Android")]
  public class AndroidController : ApiController
  {
    // POST api/Android/Register
    [AllowAnonymous]
    [HttpPost]
    [Route("Register")]
    public async Task<IHttpActionResult> PostRegister([FromBody]RegisterAndroidViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      IdentityUserManager userManager = Request.GetOwinContext().GetUserManager<IdentityUserManager>();
      var user = new User() { UserName = model.Email, Email = model.Email };

      IdentityResult result = await userManager.CreateAsync(user, model.Password);

      if (!result.Succeeded)
      {
        return GetErrorResult(result);
      }

      return Ok();
    }

    // GET api/Android/UserInfo
    [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
    [HttpGet]
    [Route("UserInfo")]
    public UserInfoAndroidViewModel GetUserInfo()
    {
      IUserManager userManager = new UserManager();
      var user = userManager.GetUser(User.Identity.GetUserId());

      return new UserInfoAndroidViewModel
      {
        FirstName = user.FirstName,
        LastName = user.LastName,
        ProfilePicture = user.ProfilePicture != null ? Convert.ToBase64String(user.ProfilePicture) : ""
      };
    }

    // POST api/Android/UserInfo
    [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
    [HttpPost]
    [Route("UserInfo")]
    public IHttpActionResult PostUserInfo([FromBody]UserInfoAndroidViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      IUserManager userManager = new UserManager();
      if (!model.ProfilePicture.Equals(""))
      {
        byte[] profilePicture = Convert.FromBase64String(model.ProfilePicture);
        userManager.ChangeBasicInfoAndroid(User.Identity.GetUserId(), model.FirstName, model.LastName, profilePicture);
      }
      else
      {
        userManager.ChangeBasicInfoAndroid(User.Identity.GetUserId(), model.FirstName, model.LastName);
      }
      
      return Ok();
    }

    // GET api/Android/Widgets
    [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
    [HttpGet]
    [Route("Widgets")]
    public IEnumerable<Widget> GetWidgets()
    {
      IWidgetManager widgetManager = new WidgetManager();
      var dashboard = widgetManager.GetDashboardWithAllDataForUserId(User.Identity.GetUserId());
      IEnumerable<UserWidget> widgets = dashboard.Widgets;
      widgets.All(x =>
      {
        x.Dashboard = null;
        return true;
      });
      return widgets;
    }

    #region Helpers

    private IHttpActionResult GetErrorResult(IdentityResult result)
    {
      if (result == null)
      {
        return InternalServerError();
      }

      if (!result.Succeeded)
      {
        if (result.Errors != null)
        {
          foreach (string error in result.Errors)
          {
            ModelState.AddModelError("", error);
          }
        }

        if (ModelState.IsValid)
        {
          // No ModelState errors are available to send, so just return an empty BadRequest.
          return BadRequest();
        }

        return BadRequest(ModelState);
      }

      return null;
    }

    #endregion
  }
}
