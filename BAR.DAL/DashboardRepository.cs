using BAR.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAR.BL.Domain.Widgets;

namespace BAR.DAL
{
	public class DashboardRepository : IDashboardRepository
	{
		private BarometerDbContext ctx;

		/// <summary>
		/// If uow is present then the constructor
		/// will get the context from uow.
		/// </summary>
		public DashboardRepository(UnitOfWork uow = null)
		{
			if (uow == null) ctx = new BarometerDbContext();
			else ctx = uow.Context;
		}

		/// <summary>
		/// Gives back a list of all the dashboards.
		/// </summary>
		public IEnumerable<Dashboard> ReadAllDashboards()
		{
			return ctx.Dashboards.AsEnumerable();
		}

		/// <summary>
		/// Gives back a list of all the widgets.
		/// </summary>
		public IEnumerable<Widget> ReadAllWidgets()
		{
			return ctx.Widgets.AsEnumerable();
		}

		/// <summary>
		/// Gives back a dashboard object for a 
		/// specific dashboard id.
		/// </summary>
		public Dashboard ReadDashboard(int dashboardId)
		{
			return ctx.Dashboards.Find(dashboardId);
		}

		/// <summary>
		/// Gives back the general dashboard.
		/// 
		/// WARING
		/// We need the general-dashboard-id before we can return
		/// the general dashboard.
		/// </summary>
		public Dashboard ReadGeneralDashboard()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gives back a widget object for a 
		/// specific widget id.
		/// </summary>
		public Widget ReadWidget(int widgetId)
		{
			return ctx.Widgets.Find(widgetId);
		}

		/// <summary>
		/// Gives back a list of widgets for a
		/// specific dashboard id.
		/// </summary>
		public IEnumerable<Widget> ReadWidgetsForDashboard(int dashboardId)
		{
			return ctx.Widgets.Where(wid => wid.Dashboard.DashboardId == dashboardId).AsEnumerable();
		}
	}
}
