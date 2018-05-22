using BAR.BL.Domain.Users;

namespace BAR.UI.MVC.Models {
    public class BaseViewModel {
        public string PageTitle { get; set; } = "";
        public string ContactStreet { get; set; } = "Nationalestraat 24";
        public string ContactCity { get; set; } = "2060 Antwerpen";
        public string ContactEmail { get; set; } = "contact@kdg.be";
				public CustomizationViewModel Customization { get; set; }
        public User User { get; set; }
    }
}