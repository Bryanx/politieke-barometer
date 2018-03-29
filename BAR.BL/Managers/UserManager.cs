using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAR.BL.Domain.Users;
using BAR.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using BAR.DAL.EF;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BAR.BL.Managers
{
	public class UserManager : UserManager<User>, IUserManager
	{
		private UnitOfWorkManager uowManager;
		private UserRepository userRepo;

		/// <summary>
		/// When unit of work is present, it will effect
		/// initRepo-method. (see documentation of initRepo)
		/// </summary>
		public UserManager(UserRepository userRepository = null, UnitOfWorkManager uowManager = null) : base(userRepository)
		{
			this.uowManager = uowManager;
			this.userRepo = userRepository;
		}

		/// <summary>
		/// Creates an instance of UserManager and returns it as a callback function to Owin.
		/// </summary>
		public static UserManager Create(IdentityFactoryOptions<UserManager> options, IOwinContext context)
		{
			var manager = new UserManager(new UserRepository(context.Get<BarometerDbContext>()));
			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context.Get<BarometerDbContext>()));
			//Configure validation logic for usernames
			manager.UserValidator = new UserValidator<User>(manager)
			{
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};

			//Configure validation logic for passwords
			manager.PasswordValidator = new PasswordValidator
			{
				RequiredLength = 6,
				RequireNonLetterOrDigit = false,
				RequireDigit = true,
				RequireLowercase = false,
				RequireUppercase = false,
			};

			//Configure user lockout defaults
			manager.UserLockoutEnabledByDefault = true;
			manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
			manager.MaxFailedAccessAttemptsBeforeLockout = 5;

			//Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
			manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User>
			{
				MessageFormat = "Your security code is {0}"
			});
			manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User>
			{
				Subject = "Security Code",
				BodyFormat = "Your security code is {0}"
			});
			manager.EmailService = new EmailService();
			manager.SmsService = new SmsService();
			var dataProtectionProvider = options.DataProtectionProvider;
			if (dataProtectionProvider != null)
			{
				manager.UserTokenProvider =
						new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
			}

			//Add roles
			AddRoles(roleManager);
			return manager;
		}

		/// <summary>
		/// Returns a list of all users.
		/// 
		/// WARNING
		/// This method uses identity
		/// InitRepo() should not be called on in this method.
		/// if so, it may produce errors.
		/// 
		/// NOTE
		/// if you plan to use identity, then the repository
		/// will already be known. because it was initialized in the constructor.
		/// if not then a new repository shall be created via the initRepo() method.
		/// </summary>
		public IEnumerable<User> GetAllUsers()
		{
			if (userRepo == null) InitRepo();
			return userRepo.ReadAllUsers();
		}

		/// <summary>
		/// Returns a user for a specific userId.
		/// 
		/// WARNING
		/// This method uses identity
		/// InitRepo() should not be called on in this method.
		/// if so, it may produce errors.
		/// 
		/// NOTE
		/// if you plan to use identity, then the repository
		/// will already be known. because it was initialized in the constructor.
		/// if not then a new repository shall be created via the initRepo() method.
		/// </summary>
		public User GetUser(string userId)
		{
			if (userRepo == null) InitRepo();
			return userRepo.ReadUser(userId);
		}

		private static void AddRoles(RoleManager<IdentityRole> roleManager)
		{
			if (!roleManager.RoleExists("Admin"))
			{
				//Create Admin role
				var role = new IdentityRole
				{
					Name = "Admin"
				};
				roleManager.Create(role);
			}
			//Create SuperAdmin role  
			if (!roleManager.RoleExists("SuperAdmin"))
			{
				var role = new IdentityRole
				{
					Name = "SuperAdmin"
				};
				roleManager.Create(role);

			}
			//Create User role   
			if (!roleManager.RoleExists("User"))
			{
				var role = new IdentityRole
				{
					Name = "User"
				};
				roleManager.Create(role);
			}
		}

		/// <summary>
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present
		/// </summary>
		private void InitRepo()
		{
			if (uowManager == null) userRepo = new UserRepository();
			else userRepo = new UserRepository(null, uowManager.UnitOfWork);
		}
	}
}
