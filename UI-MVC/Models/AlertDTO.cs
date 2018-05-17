using System;
using BAR.BL.Domain.Users;

namespace BAR.UI.MVC.Models
{
  public class AlertDTO
  {
    public int AlertId { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public AlertType AlertType { get; set; }
    public bool IsRead { get; set; }
    public DateTime? TimeStamp { get; set; }
    public string Name { get; set; }
    public int ItemId { get; set; }
  }
}