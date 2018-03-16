using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Users
{
  public class Alert
  {
    public int AlertId { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public AlertType AlertType { get; set; }
    public bool IsRead { get; set; }
    public DateTime? TimeStamp { get; set; }
    public Subscription Subscription { get; set; }
  }
}
