using System;

namespace BAR.BL.Domain.Users
{
	public class UserActivity
	{
		public int UserActivityId { get; set; }
		public DateTime TimeStamp { get; set; }
		public double NumberOfTimes { get; set; }
		public ActivityType ActivityType { get; set; }
	}
}
