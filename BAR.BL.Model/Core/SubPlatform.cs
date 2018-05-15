using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Core
{
	public class SubPlatform
	{
		public int SubPlatformId { get; set; }
		public string Name { get; set; }
		public int NumberOfUsers { get; set; }
		public DateTime CreationDate { get; set; }
		public List<Question> Questions { get; set; }
		public Customization Customization { get; set; }
        //The interval of the timer in minutes.
        public int Interval { get; set; }
        //When the timer starts running every day.
        public string SetTime { get; set; }
    }
}
