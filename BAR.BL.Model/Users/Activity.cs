using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Users
{
	public class Activity
	{
		public int ActivityId { get; set; }
		public DateTime TimeStamp { get; set; }
		public bool IsUserActivity { get; set; }
	}
}
