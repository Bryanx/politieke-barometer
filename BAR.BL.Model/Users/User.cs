using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Users
{
	public class User : IdentityUser
	{
		//Extra properties that are not included in IdentityUser
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EmailAdress { get; set; }
		public bool AlertsViaEmail { get; set; }
		public bool AlertsViaWebsite { get; set; }
		public bool WeeklyReviewViaEmail { get; set; }
		public Gender Gender { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public Area Area { get; set; }
		public ICollection<Activity> Activities { get; set; }
		public bool Deleted { get; set; }

		//Method for cookie verification (maybe removed later from domain)
		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
		{
			//Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			return userIdentity;
		}
	}
}
