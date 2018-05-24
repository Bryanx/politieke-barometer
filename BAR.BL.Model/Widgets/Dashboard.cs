
using BAR.BL.Domain.Users;
using System.Collections.Generic;

namespace BAR.BL.Domain.Widgets
{
	public class Dashboard
	{
		public int DashboardId { get; set; }
		public DashboardType DashboardType { get; set; }
		public User User { get; set; }
		public ICollection<UserWidget> Widgets { get; set; }
	}
}
