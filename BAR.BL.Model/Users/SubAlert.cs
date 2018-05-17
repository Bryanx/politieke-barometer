using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Users
{
	public class SubAlert : Alert
	{
		public Subscription Subscription { get; set; }
    public bool IsSend { get; set; }
  }
}
