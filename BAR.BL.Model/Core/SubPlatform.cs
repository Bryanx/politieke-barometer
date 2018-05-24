using System;
using System.Collections.Generic;

namespace BAR.BL.Domain.Core
{
	public class SubPlatform
	{
		public int SubPlatformId { get; set; }
		public string Name { get; set; }
		public int NumberOfUsers { get; set; }
		public DateTime CreationDate { get; set; }
		public ICollection<Question> Questions { get; set; }
		public Customization Customization { get; set; }
		public DateTime? LastUpdatedWeeklyReview { get; set; }
		public DateTime? LastUpdatedActivities { get; set; }
	}
}
