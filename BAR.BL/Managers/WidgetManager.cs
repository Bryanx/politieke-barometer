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
	public class WidgetManager : IWidgetManager
	{
		private IWidgetRepository dashboardRepo;
		private UnitOfWorkManager uowManager;

		/// <summary>
		/// When unit of work is present, it will effect
		/// initRepo-method. (see documentation of initRepo)
		/// </summary>
		public WidgetManager(UnitOfWorkManager uowManager = null)
		{
			this.uowManager = uowManager;
		}

		/// <summary>
		/// Creates a widget based on the parameters
		/// and links that widget to a dasboard.
		/// </summary>
		public UserWidget CreateWidget(int dashboardId, WidgetType widgetType, string title, int rowNbr, int colNbr, int rowspan = 1, int colspan = 1)
		{
			InitRepo();

			UserWidget widget = new UserWidget()
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
			UserWidget widgetToRemove = GetWidget(widgetId);
			if (widgetToRemove != null) dashboardRepo.DeleteWidget(widgetToRemove);
		}

		/// <summary>
		/// Gives back a widget for a
		/// specific widgetId.
		/// </summary>
		public UserWidget GetWidget(int widgetId)
		{
			InitRepo();
			return dashboardRepo.ReadWidget(widgetId);
		}

		/// <summary>
		/// Gives back a list of widgets
		/// for a specific dashboard.
		/// </summary>
		public IEnumerable<UserWidget> GetWidgets(int dashboardId)
		{
			InitRepo();
			return dashboardRepo.ReadWidgetsForDashboard(dashboardId);
		}

		/// <summary>
		/// Updates the position of the widget.
		/// </summary>
		public UserWidget ChangeWidgetPos(int widgetId, int rowNbr, int colNbr, int rowspan = 1, int colspan = 1)
		{
			InitRepo();

			//get widget
			UserWidget widgetToUpdate = GetWidget(widgetId);
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
		public UserWidget ChangeWidgetTitle(int widgetId, string title)
		{
			InitRepo();

			//get widget
			UserWidget widgetToUpdate = GetWidget(widgetId);
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
				Widgets = new List<UserWidget>(),
				Activities = new List<Activity>()
			};
			
			//Update database
			dashboardRepo.UpdateDashboard(dashboard);
			uowManager.Save();

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
			if (uowManager == null) dashboardRepo = new WidgetRepository();
			else dashboardRepo = new WidgetRepository(uowManager.UnitOfWork);
		}	
	}
}
