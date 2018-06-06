using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAR.UI.MVC.Controllers
{
	public class Itemstats
	{
		public int Male { get; set; }
		public int Female { get; set; }
		public int GenderUnknown { get; set; }

		public int Old { get; set; }
		public int Young { get; set; }
		public int AgeUnknown { get; set; }
	}
}