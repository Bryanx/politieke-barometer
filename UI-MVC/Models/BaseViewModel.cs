using BAR.BL.Domain.Users;

namespace BAR.UI.MVC.Models {
    public class BaseViewModel {
        public string PageTitle { get; set; } = "";
        public string ContactStreet { get; set; } = "Nationalestraat 24";
        public string ContactCity { get; set; } = "2060 Antwerpen";
        public string ContactEmail { get; set; } = "contact@kdg.be";
        public string PersonAlias { get; set; }
        public string PersonsAlias { get; set; }
        public string OrganisationAlias { get; set; }
        public string OrganisationsAlias { get; set; }
        public string ThemeAlias { get; set; }
        public string ThemesAlias { get; set; }
        public User User { get; set; }
    }
}