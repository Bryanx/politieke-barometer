using BAR.BL.Domain.Users;
using BAR.BL.Domain.Widgets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BAR.UI.MVC.Models
{
  public class UserInfoAndroidViewModel
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ProfilePicture { get; set; }
  }

  public class RegisterAndroidViewModel
  {
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
  }
}