using System.Collections.Generic;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;

namespace BAR.UI.MVC.Models {
    public class UserSubscribedPeopleDTO {
        public User User { get; set; }
        public IEnumerable<PersonDTO> People { get; set; }
    }
}