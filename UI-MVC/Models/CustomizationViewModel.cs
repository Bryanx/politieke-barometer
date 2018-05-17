using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BAR.UI.MVC.Models
{
	public class CustomizationViewModel : BaseViewModel
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