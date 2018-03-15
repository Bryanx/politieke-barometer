
using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Widgets
{
  public class Dashboard
  {
    public int DashboardId { get; set; }
    public DashboardType DashboardType { get; set; }
    public IEnumerable<Activity> Activities { get; set; }
  }
}
