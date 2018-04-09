﻿using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BAR.UI.MVC.Models
{
  public class ExternalLoginConfirmationViewModel : BaseViewModel
  {
    [Required]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
    [Display(Name = "Voornaam")]
    public string Firstname { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
    [Display(Name = "Achternaam")]
    public string Lastname { get; set; }

    [Required]
    [Display(Name = "Geslacht")]
    public Gender Gender { get; set; }

    [Required]
    [Display(Name = "Geboortedatum")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
    public DateTime DateOfBirth { get; set; }

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
    [Display(Name = "Email")]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Wachtwoord")]
    public string Password { get; set; }

    [Display(Name = "Onthouden?")]
    public bool RememberMe { get; set; }
  }

  public class RegisterViewModel : BaseViewModel
  {
    [Required]
    [EmailAddress]
    [Display(Name = "Email*")]
    public string Email { get; set; }

    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
    [Display(Name = "Voornaam")]
    public string Firstname { get; set; }

    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
    [Display(Name = "Achternaam")]
    public string Lastname { get; set; }

    [Display(Name = "Geslacht")]
    public Gender Gender { get; set; }

    [Display(Name = "Geboortedatum")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
    public DateTime? DateOfBirth { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Wachtwoord*")]
    public string Password { get; set; }
  }

  public class ResetPasswordViewModel : BaseViewModel
  {
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Wachtwoord")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Wachtwoord bevestigen")]
    [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    public string Code { get; set; }
  }

  public class ForgotPasswordViewModel : BaseViewModel
  {
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }
  }

  public class SettingsViewModel : BaseViewModel
  {
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
    [Display(Name = "Voornaam")]
    public string Firstname { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
    [Display(Name = "Achternaam")]
    public string Lastname { get; set; }

    [Required]
    [Display(Name = "Meldingen via email")]
    public bool AlertsViaEmail { get; set; }

    [Required]
    [Display(Name = "Meldingen via website")]
    public bool AlertsViaWebsite { get; set; }

    [Required]
    [Display(Name = "Weekly review via email")]
    public bool WeeklyReviewViaEmail { get; set; }

    [Required]
    [Display(Name = "Geslacht")]
    public Gender Gender { get; set; }

    [Display(Name = "Geboortedatum")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
    public DateTime DateOfBirth { get; set; }

    [Display(Name = "Woonplaats")]
    public int SelectedAreaId { get; set; }
    public IEnumerable<SelectListItem> Areas { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Huidig achtwoord")]
    public string Password { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Nieuw wachtwoord")]
    public string PasswordNew { get; set; }
  }
}