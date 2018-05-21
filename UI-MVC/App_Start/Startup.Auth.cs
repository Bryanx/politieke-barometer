using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.DAL.EF;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Configuration;
using System.Net.Http;
using webapi.Providers;

namespace BAR.UI.MVC
{
  public partial class Startup
  {
    public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
    public static string PublicClientId { get; private set; }

    public void ConfigureAuth(IAppBuilder app)
    {
      // Configure the db context, user manager and signin manager to use a single instance per request
      app.CreatePerOwinContext(BarometerDbContext.Create);
      app.CreatePerOwinContext<IdentityUserManager>(IdentityUserManager.Create);
      app.CreatePerOwinContext<SignInManager>(SignInManager.Create);

      // Enable the application to use a cookie to store information for the signed in user
      // and to use a cookie to temporarily store information about a user logging in with a third party login provider
      // Configure the sign in cookie
      app.UseCookieAuthentication(new CookieAuthenticationOptions
      {
        AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
        LoginPath = new PathString("/User/Login"),
        Provider = new CookieAuthenticationProvider
        {
          // Enables the application to validate the security stamp when the user logs in.
          // This is a security feature which is used when you change a password or add an external login to your account.  
          OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<IdentityUserManager, User>(
                  validateInterval: TimeSpan.FromMinutes(30),
                  regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
        }
      });
      app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

      // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
      app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

      // Enables the application to remember the second login verification factor such as phone or email.
      // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
      // This is similar to the RememberMe option when you log in.
      app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

      app.UseFacebookAuthentication(new FacebookAuthenticationOptions()
      {
        AppId = ConfigurationManager.AppSettings["FacebookAppId"],
        AppSecret = ConfigurationManager.AppSettings["FacebookAppSecret"],
        BackchannelHttpHandler = new HttpClientHandler(),
        UserInformationEndpoint = "https://graph.facebook.com/v2.8/me?fields=id,name,email,first_name,last_name,picture",
        Scope = { "public_profile" },
        Provider = new FacebookAuthenticationProvider()
        {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
			OnAuthenticated = async context =>
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
		  {
            context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
            foreach (var claim in context.User)
            {
              var claimType = string.Format("urn:facebook:{0}", claim.Key);
              string claimValue = claim.Value.ToString();
              if (!context.Identity.HasClaim(claimType, claimValue))
                context.Identity.AddClaim(new System.Security.Claims.Claim(claimType, claimValue, "XmlSchemaString", "Facebook"));
            }
          }
        }
      });

      //Configure the application for OAuth based flow
      PublicClientId = "self";
      OAuthOptions = new OAuthAuthorizationServerOptions
      {
        TokenEndpointPath = new PathString("/Token"),
        Provider = new ApplicationOAuthProvider(PublicClientId),
        AuthorizeEndpointPath = new PathString("/api/Android/ExternalLogin"),
        AccessTokenExpireTimeSpan = TimeSpan.FromDays(365),
        //For development
        AllowInsecureHttp = true
      };

      //Enable the application to use bearer tokens to authenticate users
      app.UseOAuthBearerTokens(OAuthOptions);
    }
  }
}
