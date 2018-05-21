using BAR.BL.Domain.Widgets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

  public class UserWidgetViewModel
  {
    public int WidgetId { get; set; }
    public string Title { get; set; }
    public int ColumnSpan { get; set; }
    public int RowSpan { get; set; }
    public int ColumnNumber { get; set; }
    public int RowNumber { get; set; }
    public DateTime Timestamp { get; set; }
    public WidgetType WidgetType { get; set; }
    public int DashboardId { get; set; }
    public GraphType GraphType { get; set; }
    public ICollection<int> ItemIds { get; set; }
    public List<WidgetDataDTO> WidgetDataDtos { get; set; }
  }

  public class AlertViewModel
  {
    public int AlertId { get; set; }
    public String ItemName { get; set; }
  }

  public class DeviceTokenViewModel
  {
    public String DeviceToken { get; set; }
  }
  
  public class NotificationMessageViewModel
  {
    public String Title { get; set; }
    public String Message { get; set; }
  }
}