using BAR.BL.Domain;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAR.UI.MVC.Models
{

  public class AlertVM
  {
    public int AlertId { get; set; }
    //public string Subject { get; set; }
    //public string Message { get; set; }
    //public AlertType AlertType { get; set; }
    public bool IsRead { get; set; }
    public DateTime? TimeStamp { get; set; }
    public Item item { get; set; }
    public User user { get; set; }

    //public Subscription { get; set; }

  }

  public class Subscription
  {
    public int SubscriptonId { get; set; }
    public List<Alert> Alerts { get; set; }
  }
}