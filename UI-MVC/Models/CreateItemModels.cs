using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BAR.UI.MVC.App_GlobalResources;

namespace BAR.UI.MVC.Models
{
	public class CreateItemModels
	{
		public class CreatePersonModel : BaseViewModel
		{
			[Required]
			[Display(Name = "Name", ResourceType = typeof(Resources))]
			public string Name { get; set; }
		}
	}
}