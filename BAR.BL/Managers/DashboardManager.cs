using System;
using System.Collections.Generic;
using BAR.BL.Domain.Widgets;
using BAR.DAL;
using BAR.BL.Domain.Users;

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
		public Widget CreateWidget(int dashboardId, WidgetType widgetType, string title, int rowNbr, int colNbr, int rowspan = 1, int colspan = 1)
		{
			InitRepo();

			Widget widget = new Widget()
			{
				WidgetType = widgetType,
				Title = title,
				RowNumber = rowNbr,
				ColumnNumber = colNbr,
				RowSpan = rowspan,
				ColumnSpan = colspan,
			};
			//repo autmaticly links widget to dashboard
			dashboardRepo.CreateWidget(widget, dashboardId);

			return widget;
		}

		/// <summary>
		/// Deletes a widget from the dashboard.
		/// </summary>
		public void RemoveWidget(int widgetId)
		{
			InitRepo();
			Widget widgetToRemove = GetWidget(widgetId);
			if (widgetToRemove != null) dashboardRepo.DeleteWidget(widgetToRemove);
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
		public Widget ChangeWidgetPos(int widgetId, int rowNbr, int colNbr, int rowspan = 1, int colspan = 1)
		{
			InitRepo();

			//get widget
			Widget widgetToUpdate = GetWidget(widgetId);
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
		public Widget ChangeWidgetTitle(int widgetId, string title)
		{
			InitRepo();

			//get widget
			Widget widgetToUpdate = GetWidget(widgetId);
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
		/// Creates a new dashboard based on the user.
		/// 
		/// NOTE
		/// THIS METHOD USES UNIT OF WORK
		/// </summary>
		public Dashboard CreateDashboard(string userId, DashboardType dashType)
		{
			uowManager = new UnitOfWorkManager();
			InitRepo();

			//Get user
			UserManager userManager = new UserManager(uowManager);
			User user = userManager.GetUser(userId);
			if (user == null) return null;

			//Create dashboard
			Dashboard dashboard = new Dashboard()
			{
				DashboardType = dashType,
				User = user,
				Widgets = new List<Widget>(),
				Activities = new List<Activity>()
			};
			
			//Update database
			dashboardRepo.UpdateDashboard(dashboard);
			uowManager.Save();
			uowManager = null;

			return dashboard;
		}

		/// <summary>
		/// Deletes a dashboard from the database.
		/// </summary>
		public void RemoveDashboard(int dashboardId)
		{
			InitRepo();
			dashboardRepo.DeleteDashboard(dashboardId);
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
