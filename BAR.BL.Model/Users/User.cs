using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Users
{
	public class User : IdentityUser
	{
		//Extra properties that are not included in IdentityUser
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool AlertsViaEmail { get; set; }
		public bool AlertsViaWebsite { get; set; }
		public bool WeeklyReviewViaEmail { get; set; }
		public bool IsActive { get; set; }
		public Gender Gender { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public Area Area { get; set; }
		public bool Deleted { get; set; }
		public byte[] ProfilePicture { get; set; }
		public ICollection<UserAlert> Alerts { get; set; }
		public string DeviceToken { get; set; }

		//Method for cookie verification (maybe removed later from domain)
		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType = DefaultAuthenticationTypes.ApplicationCookie)
		{
			//Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
			return userIdentity;
		}
	}
}
