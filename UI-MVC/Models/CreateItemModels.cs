﻿using System;
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

		public class CreateOrganisationModel : BaseViewModel
		{
			[Required]
			[Display(Name = "Name", ResourceType = typeof(Resources))]
			public string Name { get; set; }
		}

		public class CreateThemeModel : BaseViewModel
		{
			[Required]
			[Display(Name = "Name", ResourceType = typeof(Resources))]
			public string Name { get; set; }

			[Required]
			[Display(Name = "Keywords", ResourceType = typeof(Resources))]
			public string Keywords { get; set; }
		}
	}
}