﻿using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
 using BAR.UI.MVC.App_GlobalResources;

namespace BAR.UI.MVC.Models
{
  public class ExternalLoginConfirmationViewModel : BaseViewModel
  {
    [Required]
    [Display(Name = "Email", ResourceType = typeof(Resources))]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
    [Display(Name = "Firstname", ResourceType = typeof(Resources))]
    public string Firstname { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
    [Display(Name = "Lastname", ResourceType = typeof(Resources))]
    public string Lastname { get; set; }

    [Required]
    [Display(Name = "Gender", ResourceType = typeof(Resources))]
    public Gender Gender { get; set; }

    [Required]
    [Display(Name = "DateOfBirth", ResourceType = typeof(Resources))]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
    public DateTime DateOfBirth { get; set; }

    [Display(Name = "ProfilePicture", ResourceType = typeof(Resources))]
    public byte[] ImageData { get; set; }

  }

  public class ExternalLoginListViewModel : BaseViewModel
  {
    public string ReturnUrl { get; set; }
  }

  public class ForgotViewModel
  {
    [Required]
    [Display(Name = "Email")]
    public string Email { get; set; }
  }

  public class LoginViewModel : BaseViewModel
  {
    [Required]
    [Display(Name = "Email", ResourceType = typeof(Resources))]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password", ResourceType = typeof(Resources))]
    public string Password { get; set; }

    [Display(Name = "RememberMeQuestion", ResourceType = typeof(Resources))]
    public bool RememberMe { get; set; }
  }

  public class RegisterViewModel : BaseViewModel
  {
    [Required]
    [EmailAddress]
    [Display(Name = "Email", ResourceType = typeof(Resources))]
    public string Email { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
    [Display(Name = "Firstname", ResourceType = typeof(Resources))]
    public string Firstname { get; set; }

    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
    [Display(Name = "Lastname", ResourceType = typeof(Resources))]
    public string Lastname { get; set; }

    [Display(Name = "Gender", ResourceType = typeof(Resources))]
    public Gender Gender { get; set; }

    [Display(Name = "DateOfBirth", ResourceType = typeof(Resources))]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
    public DateTime? DateOfBirth { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password", ResourceType = typeof(Resources))]
    public string Password { get; set; }
  }

  public class ResetPasswordViewModel : BaseViewModel
  {
    [Required]
    [EmailAddress]
    [Display(Name = "Email", ResourceType = typeof(Resources))]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password", ResourceType = typeof(Resources))]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources))]
    [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceType = typeof(Resources),
      ErrorMessageResourceName = "PasswordNotMatch")]
    public string ConfirmPassword { get; set; }

    public string Code { get; set; }
  }

  public class ForgotPasswordViewModel : BaseViewModel
  {
    [Required]
    [EmailAddress]
    [Display(Name = "Email", ResourceType = typeof(Resources))]
    public string Email { get; set; }
  }

  public class SettingsViewModel : BaseViewModel
  {
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
    [Display(Name = "Firstname", ResourceType = typeof(Resources))]
    public string Firstname { get; set; }

    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
    [Display(Name = "Lastname", ResourceType = typeof(Resources))]
    public string Lastname { get; set; }

    [Required]
    [Display(Name = "AlertsViaEmail", ResourceType = typeof(Resources))]
    public bool AlertsViaEmail { get; set; }

    [Required]
    [Display(Name = "AlertsViaWebsite", ResourceType = typeof(Resources))]
    public bool AlertsViaWebsite { get; set; }

    [Required]
    [Display(Name = "WeeklyReviewViaEmail", ResourceType = typeof(Resources))]
    public bool WeeklyReviewViaEmail { get; set; }

    [Required]
    [Display(Name = "Gender", ResourceType = typeof(Resources))]
    public Gender Gender { get; set; }

    [Display(Name = "DateOfBirth", ResourceType = typeof(Resources))]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
    public DateTime DateOfBirth { get; set; }

    [Display(Name = "City", ResourceType = typeof(Resources))]
    public int SelectedAreaId { get; set; }
    public IEnumerable<SelectListItem> Areas { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "CurrentPassword", ResourceType = typeof(Resources))]
    public string Password { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "NewPassword", ResourceType = typeof(Resources))]
    public string PasswordNew { get; set; }

    [Display(Name = "ProfilePicture", ResourceType = typeof(Resources))]
    public byte[] ProfilePicture { get; set; }
  }
}