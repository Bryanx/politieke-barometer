﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BAR.BL.Domain.Core
{
	/// <summary>
	/// NOTE
	/// All colors are hex-values
	/// </summary>
	public class Customization
	{
		public int CustomizationId { get; set; }

		//Colors
		public string PrimaryColor { get; set; }
		public string SecondairyColor { get; set; }
		public string TertiaryColor { get; set; }
		public string BackgroundColor { get; set; }
		public string TextColor { get; set; }

		//Navbar and title text
		public string PersonAlias { get; set; }
		public string PersonsAlias { get; set; }
		public string OrganisationAlias { get; set; }
		public string OrganisationsAlias { get; set; }
		public string ThemeAlias { get; set; }
		public string ThemesAlias { get; set; }

		//Privacy
		public string PrivacyTitle { get; set; }
		public string PrivacyText { get; set; }

		//FAQ
		public string FAQTitle { get; set; }

		//Contact properties
		public string StreetAndHousenumber { get; set; }
		public string Zipcode { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public string Email { get; set; }

		//Images
		public byte[] HeaderImage { get; set; }
	}
}
