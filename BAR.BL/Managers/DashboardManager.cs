using System;
using System.Collections.Generic;
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
		public Widget CreateWidget(int dashboardId, string title, int rowNbr, int colNbr, int rowspan = 1, int colspan = 1)
		{
			InitRepo();

			Widget widget = new Widget()
			{
				Title = title,
				RowNumber = rowNbr,
				ColumnNumber = colNbr,
				RowSpan = rowspan,
				ColumnSpan = colspan,
			};
			//repo autmaticly links widget to dashboard in
			dashboardRepo.CreateWidget(widget, dashboardId);

			return widget;
		}

		/// <summary>
		/// Deletes a widget from the dashboard.
		/// </summary>
		public void RemoveWidget(int widgetId)
		{
			InitRepo();
			dashboardRepo.DeleteWidget(widgetId);
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
		/// Updates the position of the widget.
		/// </summary>
		public Widget UpdateWidgetPos(int widgetId, int rowNbr, int colNbr, int rowspan = 1, int colspan = 1)
		{
			InitRepo();

			//get widget
			Widget widgetToUpdate = dashboardRepo.ReadWidget(widgetId);
			if (widgetToUpdate == null) return null;

			//update widget
			widgetToUpdate.RowNumber = rowNbr;
			widgetToUpdate.ColumnNumber = colNbr;
			widgetToUpdate.RowSpan = rowspan;
			widgetToUpdate.ColumnSpan = colspan;

			//update database
			dashboardRepo.UpdateWidget(widgetToUpdate);

			return widgetToUpdate;
		}

		/// <summary>
		/// Updates the position of the widget.
		/// </summary>
		public Widget UpdateWidgetTitle(int widgetId, string title)
		{
			InitRepo();

			//get widget
			Widget widgetToUpdate = dashboardRepo.ReadWidget(widgetId);
			if (widgetToUpdate == null) return null;

			//update widget
			widgetToUpdate.Title = title;

			//update database
			dashboardRepo.UpdateWidget(widgetToUpdate);

			return widgetToUpdate;
		}

		/// <summary>
		/// Gives back a dashboard with their widgets.
		/// </summary>
		public Dashboard GetDashboard(int dashboardId)
		{
			InitRepo();
			return dashboardRepo.ReadDashboardWithWidgets(dashboardId);
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
