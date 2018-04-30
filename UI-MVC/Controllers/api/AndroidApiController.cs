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
    public async Task<IHttpActionResult> Register([FromBody]RegisterBindingModel model)
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
    [Route("UserInfo")]
    public UserInfoViewModel GetUserInfo()
    {
      //ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
      IUserManager userManager = new UserManager();
      var user = userManager.GetUser(User.Identity.GetUserId());

      return new UserInfoViewModel
      {
        FirstName = user.FirstName,
        LastName = user.LastName,
        ProfilePicture = Convert.ToBase64String(user.ProfilePicture)
      };
    }

    // POST api/Android/Logout
    [Route("Logout")]
    public IHttpActionResult Logout()
    {
      Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
      return Ok();
    }

    // GET api/Android/ExternalLogin
    [OverrideAuthentication]
    [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
    [AllowAnonymous]
    [Route("ExternalLogin", Name = "ExternalLogin")]
    public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
    {
      IdentityUserManager userManager = Request.GetOwinContext().GetUserManager<IdentityUserManager>();

      if (error != null)
      {
        return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
      }

      if (!User.Identity.IsAuthenticated)
      {
        return new ChallengeResult(provider, this);
      }

      ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

      if (externalLogin == null)
      {
        return InternalServerError();
      }

      if (externalLogin.LoginProvider != provider)
      {
        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        return new ChallengeResult(provider, this);
      }

      var user = await userManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
          externalLogin.ProviderKey));

      bool hasRegistered = user != null;

      if (hasRegistered)
      {
        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

        ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
           OAuthDefaults.AuthenticationType);
        ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(userManager,
            CookieAuthenticationDefaults.AuthenticationType);

        AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
        Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
      }
      else
      {
        IEnumerable<Claim> claims = externalLogin.GetClaims();
        ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
        Authentication.SignIn(identity);
      }

      return Ok();
    }

    // GET api/Android/ExternalLogins?returnUrl=%2F&generateState=true
    [AllowAnonymous]
    [Route("ExternalLogins")]
    public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
    {
      IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
      List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

      string state;

      if (generateState)
      {
        const int strengthInBits = 256;
        state = RandomOAuthStateGenerator.Generate(strengthInBits);
      }
      else
      {
        state = null;
      }

      foreach (AuthenticationDescription description in descriptions)
      {
        ExternalLoginViewModel login = new ExternalLoginViewModel
        {
          Name = description.Caption,
          Url = Url.Route("ExternalLogin", new
          {
            provider = description.AuthenticationType,
            response_type = "token",
            client_id = Startup.PublicClientId,
            redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
            state = state
          }),
          State = state
        };
        logins.Add(login);
      }

      return logins;
    }

    #region Helpers

    private IAuthenticationManager Authentication
    {
      get { return Request.GetOwinContext().Authentication; }
    }

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

    private class ExternalLoginData
    {
      public string LoginProvider { get; set; }
      public string ProviderKey { get; set; }
      public string UserName { get; set; }

      public IList<Claim> GetClaims()
      {
        IList<Claim> claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

        if (UserName != null)
        {
          claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
        }

        return claims;
      }

      public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
      {
        if (identity == null)
        {
          return null;
        }

        Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

        if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
            || String.IsNullOrEmpty(providerKeyClaim.Value))
        {
          return null;
        }

        if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
        {
          return null;
        }

        return new ExternalLoginData
        {
          LoginProvider = providerKeyClaim.Issuer,
          ProviderKey = providerKeyClaim.Value,
          UserName = identity.FindFirstValue(ClaimTypes.Name)
        };
      }
    }

    private static class RandomOAuthStateGenerator
    {
      private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

      public static string Generate(int strengthInBits)
      {
        const int bitsPerByte = 8;

        if (strengthInBits % bitsPerByte != 0)
        {
          throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
        }

        int strengthInBytes = strengthInBits / bitsPerByte;

        byte[] data = new byte[strengthInBytes];
        _random.GetBytes(data);
        return HttpServerUtility.UrlTokenEncode(data);
      }
    }

    #endregion
  }
}
