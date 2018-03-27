using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAR.BL.Domain.Widgets;
using BAR.DAL;

namespace BAR.BL.Managers
{
	/// <summary>
	/// Responsable for managing widgets
	/// and their dashboards 
	/// </summary>
	public class DashboardManager : IDashboardManager
	{
		private IDashboardRepository dashboardRepo;
		private UnitOfWorkManager uowManager;

		/// <summary>
		/// When unit of work is present, it will effect
		/// initRepo-method. (see documentation of initRepo)
		/// </summary>
		public DashboardManager(UnitOfWorkManager uowManager = null)
		{
			this.uowManager = uowManager;
		}

		/// <summary>
		/// Creates a widget based on the parameters
		/// and links that widget to a dasboard.
		/// </summary>
		public Widget CreateWidget(int dashboardId, string title, int rowNbr, int colNbr, int rowspan, int colspan)
		{
			InitRepo();

			//get dashboard
			Dashboard dashboard = dashboardRepo.ReadDashboard(dashboardId);
			if (dashboard == null) return null;

			//create widget
			Widget widget = new Widget()
			{
				Title = title,
				RowNumber = rowNbr,
				ColumnNumber = colNbr,
				RowSpan = rowspan,
				ColumnSpan = colspan,
				Dashboard = dashboard
			};
			dashboard.Widgets.Add(widget);

			//update database
			dashboardRepo.UpdateDashboard(dashboard);

			return widget;
		}

		/// <summary>
		/// Gives back a widget for a
		/// specific widgetId.
		/// </summary>
		public Widget GetWidget(int widgetId)
		{
			InitRepo();
			return dashboardRepo.ReadWidget(widgetId);
		}

		/// <summary>
		/// Gives back a list of widgets
		/// for a specific dashboard.
		/// </summary>
		public IEnumerable<Widget> GetWidgets(int dashboardId)
		{
			InitRepo();
			return dashboardRepo.ReadWidgetsForDashboard(dashboardId);
		}

		/// <summary>
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present.
		/// </summary>
		private void InitRepo()
		{
			if (uowManager == null) dashboardRepo = new DashboardRepository();
			else dashboardRepo = new DashboardRepository(uowManager.UnitOfWork);
		}
	}
}
