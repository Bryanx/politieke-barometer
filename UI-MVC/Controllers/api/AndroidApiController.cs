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
using System.Net;
using AutoMapper;

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
      User user = new User() { UserName = model.Email, Email = model.Email };

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
    public IHttpActionResult GetUserInfo()
    {
      IUserManager userManager = new UserManager();
      User user = userManager.GetUser(User.Identity.GetUserId());

      UserInfoAndroidViewModel model = new UserInfoAndroidViewModel
      {
        FirstName = user.FirstName,
        LastName = user.LastName,
        ProfilePicture = user.ProfilePicture != null ? Convert.ToBase64String(user.ProfilePicture) : ""
      };

      return Ok(model);
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
    public IHttpActionResult GetWidgets()
    {
      IWidgetManager widgetManager = new WidgetManager();
      ItemManager itemManager = new ItemManager();

      Dashboard dashboard = widgetManager.GetDashboardWithAllDataForUserId(User.Identity.GetUserId());
      if (dashboard == null) return NotFound();

      //Get all widgets for user
      List<UserWidget> userWidgets = widgetManager.GetWidgetsForDashboard(dashboard.DashboardId).ToList();
      if (userWidgets == null || userWidgets.Count() == 0) return StatusCode(HttpStatusCode.NoContent);

      //Convert all widget to userWidgets (DTO's).
      List<UserWidgetViewModel> userWidgetDtos = Mapper.Map(userWidgets, new List<UserWidgetViewModel>());

      foreach (UserWidgetViewModel userWidgetVM in userWidgetDtos)
      {
        foreach (int itemId in userWidgetVM.ItemIds)
        {
          //Get the topic of the graph of the userwidget.
          string keyValue = widgetManager.GetWidgetWithAllData(userWidgetVM.WidgetId)?.WidgetDatas.FirstOrDefault()?.KeyValue;

          //Get all widgets for each item in userWidgets.
          IEnumerable<Widget> widgetsForItem = widgetManager.GetAllWidgetsWithAllDataForItem(itemId);

          //Check if these widgets have graph data (WidgetData) on this topic, if they do add this data to the userWidget.
          IEnumerable<WidgetData> widgetDatas = widgetsForItem.FirstOrDefault(w => w.WidgetDatas.Any(wd => wd.KeyValue == keyValue)).WidgetDatas;

          //Convert the graphdata to a DTO.
          List<WidgetDataDTO> widgetDataDtos = Mapper.Map(widgetDatas, new List<WidgetDataDTO>());

          //Link the graphdata to the corresponding item.
          widgetDataDtos.First().ItemName = itemManager.GetItem(itemId).Name;

          if (userWidgetVM.WidgetDataDtos == null)
          {
            userWidgetVM.WidgetDataDtos = widgetDataDtos;
          }
          else
          {
            userWidgetVM.WidgetDataDtos.AddRange(widgetDataDtos);
          }
        }
      }

      return Ok(userWidgetDtos);
    }

    // GET api/Android/Alerts
    [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
    [HttpGet]
    [Route("Alerts")]
    public IHttpActionResult GetAlerts()
    {
      ISubscriptionManager subManager = new SubscriptionManager();
      IEnumerable<UserAlert> userAlerts = subManager.GetUserAlerts(User.Identity.GetUserId());
      IEnumerable<SubAlert> subAlerts = subManager.GetSubAlerts(User.Identity.GetUserId());
      if (userAlerts == null || subAlerts == null || (userAlerts.Count() == 0 && subAlerts.Count() == 0)) return StatusCode(HttpStatusCode.NoContent);

      //Made DTO class to prevent circular references
      List<AlertDTO> alerts = new List<AlertDTO>();
      foreach (SubAlert alert in subAlerts)
      {
        AlertDTO alertDTO = new AlertDTO()
        {
          AlertId = alert.AlertId,
          Name = alert.Subscription.SubscribedItem.Name,
          TimeStamp = alert.TimeStamp,
          IsRead = alert.IsRead,
          ItemId = alert.Subscription.SubscribedItem.ItemId
        };
        alerts.Add(alertDTO);
      }
      foreach (UserAlert alert in userAlerts)
      {
        AlertDTO alertDTO = new AlertDTO()
        {
          AlertId = alert.AlertId,
          Name = alert.Subject,
          TimeStamp = alert.TimeStamp,
          IsRead = alert.IsRead,
        };
        alerts.Add(alertDTO);
      }

      return Ok(alerts);
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
