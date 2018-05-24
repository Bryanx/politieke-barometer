using BAR.BL.Domain.Users;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace BAR.BL.Managers
{
	/// <summary>
	/// Class that will make it possible for users to login and off.
	/// </summary>
	public class SignInManager : SignInManager<User, string>
	{
		public SignInManager(IdentityUserManager userManager, IAuthenticationManager authenticationManager)
				: base(userManager, authenticationManager) { }

		/// <summary>
		/// Creates an instance of SignInManager and returns it as a callback function to Owin.
		/// </summary>
		public static SignInManager Create(IdentityFactoryOptions<SignInManager> options, IOwinContext context)
		{
			return new SignInManager(context.GetUserManager<IdentityUserManager>(), context.Authentication);
		}
	}
}
