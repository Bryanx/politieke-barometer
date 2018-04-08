using System.Collections.Generic;
using BAR.BL.Domain.Users;

namespace BAR.UI.MVC.Models {
    public class EditUserViewModel : BaseViewModel {
        public IEnumerable<User> Users { get; set; }

        public EditUserViewModel(BaseViewModel vm, IEnumerable<User> users) {
            PageTitle = vm.PageTitle;
            ContactStreet = vm.ContactStreet;
            ContactCity = vm.ContactCity;
            ContactEmail = vm.ContactEmail;
            User = vm.User;
            IsAdmin = vm.IsAdmin;
            IsSuperAdmin = vm.IsSuperAdmin;
            Users = users;
        }
    }
}