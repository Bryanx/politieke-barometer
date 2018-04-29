using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using BAR.BL.Domain.Widgets;
using BAR.DAL;
using BAR.BL.Domain.Users;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Data;

namespace BAR.BL.Managers
{
	/// <summary>
	/// Responsable for managing widgets
	/// and their dashboards 
	/// </summary>
	public class WidgetManager : IWidgetManager
	{
		private IWidgetRepository widgetRepo;
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
		public Widget AddWidget(WidgetType widgetType, string title, int rowNbr, int colNbr, List<PropertyTag> proptags, DateTime? timestamp = null, GraphType? graphType = null, int rowspan = 1, int colspan = 1, int dashboardId = -1)
		{
			InitRepo();
			Widget widget;

			//Checks if a userwidget or an itemWidget needs to be created
			if (dashboardId == -1) widget = new ItemWidget();
			else widget = new UserWidget();

			//Check if timestamp was given
			if (timestamp == null) timestamp = DateTime.Now.AddMonths(-1);

			widget.WidgetType = widgetType;
			widget.Title = title;
			widget.RowNumber = rowNbr;
			widget.ColumnNumber = colNbr;
			widget.RowSpan = rowspan;
			widget.ColumnSpan = colspan;
			widget.Timestamp = timestamp;
			widget.Items = new List<Item>();
			widget.GraphType = graphType;
			widget.PropertyTags = proptags;
			widget.WidgetData = new List<WidgetData>();

			//Update database
			if (dashboardId != -1)
			{
				Dashboard dasboardToAddWidget = widgetRepo.ReadDashboardWithWidgets(dashboardId);
				dasboardToAddWidget.Widgets.Add((UserWidget)widget);
			}
			widgetRepo.CreateWidget(widget);

			return widget;
		}

		/// <summary>
		/// Adds an item to a widget.
		/// 
		/// WARNING
		/// THIS METHOD USES UNIT OF WORK
		/// </summary>
		public Widget AddItemToWidget(int widgetId, int itemId)
		{
			uowManager = new UnitOfWorkManager();
			InitRepo();

			//Get Item
			ItemManager itemManager = new ItemManager(uowManager);
			Item itemToAdd = itemManager.GetItemWithWidgets(itemId);

			//Add item to widget
			Widget widgetToUpdate = GetWidgetWithAllItems(widgetId);
			widgetToUpdate.Items.Add(itemToAdd);

			//Update database
			widgetRepo.UpdateWidget(widgetToUpdate);

			uowManager.Save();
			uowManager = null;

			return widgetToUpdate;
		}

		/// <summary>
		/// Deletes a widget from the dashboard.
		/// </summary>
		public void RemoveWidget(int widgetId)
		{
			InitRepo();
			Widget widgetToRemove = GetWidget(widgetId);
			if (widgetToRemove != null) widgetRepo.DeleteWidget(widgetToRemove);
		}

		/// <summary>
		/// Gives back a widget for a
		/// specific widgetId.
		/// </summary>
		public Widget GetWidget(int widgetId)
		{
			InitRepo();
			return widgetRepo.ReadWidget(widgetId);
		}

		/// <summary>
		/// Gives back a list of widgets
		/// for a specific dashboard.
		/// </summary>
		public IEnumerable<UserWidget> GetWidgetsForDashboard(int dashboardId)
		{
			InitRepo();
			return widgetRepo.ReadWidgetsForDashboard(dashboardId);
		}

		/// <summary>
		/// Gives back a list of widgets
		/// for a specific item.
		/// </summary>
		/// 

		public IEnumerable<Widget> GetWidgetsForItem(int itemId)
		{
			InitRepo();
			ItemManager itemManager = new ItemManager();
			return itemManager.GetItemWithWidgets(itemId).ItemWidgets.AsEnumerable();
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
			widgetRepo.UpdateWidget(widgetToUpdate);

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
			widgetRepo.UpdateWidget(widgetToUpdate);

			return widgetToUpdate;
		}

		/// <summary>
		/// Updates a widget.
		/// </summary>
		public Widget ChangeWidget(Widget widget)
		{
			InitRepo();
			widgetRepo.UpdateWidget(widget);
			return widget;
		}

		/// <summary>
		/// Gives back a dashboard with their widgets.
		/// </summary>
		public Dashboard GetDashboard(int dashboardId)
		{
			InitRepo();
			return widgetRepo.ReadDashboardWithWidgets(dashboardId);
		}

		/// <summary>
		/// Gives back a dashboard with their widgets.
		/// </summary>
		public Dashboard GetDashboard(string userId)
		{
			InitRepo();
			Dashboard dash = widgetRepo.ReadDashboardWithWidgets(userId);
			if (dash == null) return AddDashboard(userId);
			return dash;
		}

		/// <summary>
		/// Creates a new dashboard based on the user.
		/// 
		/// NOTE
		/// THIS METHOD USES UNIT OF WORK
		/// </summary>
		public Dashboard AddDashboard(string userId = "-1", DashboardType dashType = DashboardType.Private)
		{
			uowManager = new UnitOfWorkManager();
			InitRepo();

			//Create dashboard
			Dashboard dashboard = new Dashboard()
			{
				DashboardType = dashType,
				Widgets = new List<UserWidget>(),
				Activities = new List<Activity>()
			};

			//Get user if not general dashboard
			if (!userId.Equals("-1"))
			{
				UserManager userManager = new UserManager(uowManager);
				User user = userManager.GetUser(userId);

				if (user == null) return null;
				else dashboard.User = user;
			}

			//Create database
			widgetRepo.CreateDashboard(dashboard);
			uowManager.Save();

			return dashboard;
		}

		/// <summary>
		/// Deletes a dashboard from the database.
		/// </summary>
		public void RemoveDashboard(int dashboardId)
		{
			InitRepo();
			widgetRepo.DeleteDashboard(dashboardId);
		}

		/// <summary>
		/// Gives back a widget with all the items
		/// </summary>
		public Widget GetWidgetWithAllItems(int widgetId)
		{
			InitRepo();
			return widgetRepo.ReadWidgetWithAllitems(widgetId);
		}

		/// <summary>
		/// Gives back a list with all the widgets and all the items.
		/// </summary>
		public IEnumerable<Widget> GetAllWidgetsWithAllItems()
		{
			InitRepo();
			return widgetRepo.ReadAllWidgetsWithAllItems().AsEnumerable();
		}

		/// <summary>
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present.
		/// </summary>
		private void InitRepo()
		{
			if (uowManager == null) widgetRepo = new WidgetRepository();
			else widgetRepo = new WidgetRepository(uowManager.UnitOfWork);

		}

		/// <summary>
		/// Gives back a widget with all the data.
		/// </summary>
		public Widget GetWidgetWithAllData(int widgetId)
		{
			InitRepo();
			return widgetRepo.ReadWidgetWithAllData(widgetId);
		}

		/// <summary>
		/// Gives back all the widgets with all the data.
		/// </summary>
		public IEnumerable<Widget> GetAllWidgetsWithAllData()
		{
			InitRepo();
			return widgetRepo.ReadAllWidgetsWithAllData().AsEnumerable();
		}

		/// <summary>
		/// Updates all the given widgets
		/// </summary>
		public IEnumerable<Widget> ChangeWidgets(IEnumerable<Widget> widgets)
		{
			InitRepo();
			widgetRepo.UpdateWidgets(widgets);
			return widgets;
		}
	}
}
