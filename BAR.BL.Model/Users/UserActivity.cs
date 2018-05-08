using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Users
{
	public class UserActivity
	{
		public int UserActivityId { get; set; }
		public DateTime TimeStamp { get; set; }
		public double NumberOfTimes { get; set; }
		public bool IsRegisterActivity { get; set; }
	}
}
