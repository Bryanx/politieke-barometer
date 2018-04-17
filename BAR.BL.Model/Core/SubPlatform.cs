using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using BAR.BL.Domain.Core;
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
	}
}
