using BAR.BL.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAR.UI.MVC.Models
{
	public class SourceManagement : BaseViewModel
	{
		public string Name { get; set; }
		public string Site { get; set; }
		public IEnumerable<Source> Sources { get; set; }
	}

	public class SubPlatformManagement : BaseViewModel
	{
		public string Name { get; set; }
		public IEnumerable<SubPlatformDTO> SubPlatforms { get; set; }
	}
}