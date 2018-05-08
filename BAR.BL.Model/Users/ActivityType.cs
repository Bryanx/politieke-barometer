using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Users
{
	public enum ActivityType : Byte
	{
		LoginActivity = 1,
		RegisterActivity,
		VisitActitiy
	}
}
