using System.Collections.Generic;

namespace BAR.UI.MVC.Models {
    public class UserViewModel : BaseViewModel {
        public IEnumerable<PersonDTO> People { get; set; }
    }
}