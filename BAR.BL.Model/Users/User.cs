using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Users
{
	public class User
	{
		public int UserId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EmailAdress { get; set; }
		public bool AlertsViaEmail { get; set; }
		public bool AlertsViaWebsite { get; set; }
		public bool WeeklyReviewViaEmail { get; set; }
		public Gender Gender { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public Role Role { get; set; }
		public Area Area { get; set; }
		public ICollection<Activity> Activities { get; set; }
		public bool Deleted { get; set; }
	}
}
