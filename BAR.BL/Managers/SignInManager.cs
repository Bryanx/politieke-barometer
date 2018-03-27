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
    public SignInManager(UserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
    }

    public static SignInManager Create(IdentityFactoryOptions<SignInManager> options, IOwinContext context)
    {
      return new SignInManager(context.GetUserManager<UserManager>(), context.Authentication);
    }
  }
}
