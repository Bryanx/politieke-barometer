using System.Collections.Generic;
using BAR.BL.Domain.Users;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;

namespace BAR.UI.MVC.Models
{
  public class EditUserViewModel : BaseViewModel
  {
    public IEnumerable<User> Users { get; set; }
    public IEnumerable<SelectListItem> AdminRoles { get; set; }
    public IEnumerable<SelectListItem> UserRoles { get; set; }
  }
}