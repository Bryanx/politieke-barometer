using System;
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
		[RegularExpression("^#([0-9]|[a-f]|[A-F]){6}$")]
		public string PrimaryColor { get; set; }
		[RegularExpression("^#([0-9]|[a-f]|[A-F]){6}$")]
		public string SecondairyColor { get; set; }
		[RegularExpression("^#([0-9]|[a-f]|[A-F]){6}$")]
		public string TertiaryColor { get; set; }
		[RegularExpression("^#([0-9]|[a-f]|[A-F]){6}$")]
		public string BackgroundColor { get; set; }
		[RegularExpression("^#([0-9]|[a-f]|[A-F]){6}$")]
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
	}
}
