using BAR.BL.Domain.Users;

namespace BAR.UI.MVC.Models {
    public class BaseViewModel {
        public string PageTitle { get; set; }
        public User User { get; set; }        
    }
}