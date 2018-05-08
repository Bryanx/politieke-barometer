using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Users
{
	public class UserActivity
	{
		public int ActivityId { get; set; }
		public DateTime TimeStamp { get; set; }
		public double NumberOfNewUsers { get; set; }
	}
}
