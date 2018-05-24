using System;
using BAR.BL.Domain.Users;
using BAR.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using BAR.DAL.EF;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BAR.BL.Managers
{
	/// <summary>
	/// Class which will handle all configuration related to Identity.
	/// </summary>
	public class IdentityUserManager : UserManager<User>
	{
		/// <summary>
		/// Repository is given to base class (UserManager - Identity).
		/// </summary>
		public IdentityUserManager(UserIdentityRepository userIdentityRepository) : base(userIdentityRepository) { }

		/// <summary>
		/// Creates an instance of UserManager and returns it as a callback function to Owin.
		/// </summary>
		public static IdentityUserManager Create(IdentityFactoryOptions<IdentityUserManager> options, IOwinContext context)
		{
			var identityManager = new IdentityUserManager(new UserIdentityRepository(context.Get<BarometerDbContext>()));
			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context.Get<BarometerDbContext>()));

			//Configure validation logic for usernames
			identityManager.UserValidator = new UserValidator<User>(identityManager)
			{
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};

			//Configure validation logic for passwords
			identityManager.PasswordValidator = new PasswordValidator
			{
				RequiredLength = 6,
				RequireNonLetterOrDigit = false,
				RequireDigit = true,
				RequireLowercase = false,
				RequireUppercase = false,
			};

			//Configure user lockout defaults
			identityManager.UserLockoutEnabledByDefault = true;
			identityManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
			identityManager.MaxFailedAccessAttemptsBeforeLockout = 5;

			//Register two factor authentication providers
			identityManager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User>
			{
				Subject = "Security Code",
				BodyFormat = "Your security code is {0}"
			});
			identityManager.EmailService = new EmailService();
			var dataProtectionProvider = options.DataProtectionProvider;
			if (dataProtectionProvider != null)
			{
				identityManager.UserTokenProvider =
						new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
			}

			//Add roles
			AddRoles(roleManager);
			return identityManager;
		}

		/// <summary>
		/// Checks if all roles are avaiable in database, otherwise add them.
		/// </summary>
		private static void AddRoles(RoleManager<IdentityRole> roleManager)
		{
			//Create Admin role
			if (!roleManager.RoleExists("Admin"))
			{
				IdentityRole role = new IdentityRole
				{
					Name = "Admin"
				};
				roleManager.Create(role);
			}

			//Create SuperAdmin role  
			if (!roleManager.RoleExists("SuperAdmin"))
			{
				IdentityRole role = new IdentityRole
				{
					Name = "SuperAdmin"
				};
				roleManager.Create(role);
			}

			//Create User role   
			if (!roleManager.RoleExists("User"))
			{
				IdentityRole role = new IdentityRole
				{
					Name = "User"
				};
				roleManager.Create(role);
			}
		}
	}
}