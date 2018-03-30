using BAR.BL.Domain.Users;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Managers
{
  public class SignInManager : SignInManager<User, string>
  {
    public SignInManager(IdentityUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
    }

    /// <summary>
    /// Creates an instance of SignInManager and returns it as a callback function to Owin.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static SignInManager Create(IdentityFactoryOptions<SignInManager> options, IOwinContext context)
    {
      return new SignInManager(context.GetUserManager<IdentityUserManager>(), context.Authentication);
    }
  }
}
