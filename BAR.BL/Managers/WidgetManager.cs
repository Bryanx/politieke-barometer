﻿using System;
using System.Collections.Generic;
using BAR.BL.Domain.Widgets;
using BAR.DAL;
using BAR.BL.Domain.Users;
using BAR.BL.Domain.Items;

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
		public Widget CreateWidget(WidgetType widgetType, string title, int rowNbr, int colNbr, int rowspan = 1, int colspan = 1, int dashboardId = -1)
		{
			InitRepo();
			Widget widget;

			//Checks if a userwidget or an itemWidget needs to be created
			if (dashboardId == -1)
			{
				widget = new ItemWidget()
				{
					WidgetType = widgetType,
					Title = title,
					RowNumber = rowNbr,
					ColumnNumber = colNbr,
					RowSpan = rowspan,
					ColumnSpan = colspan,
					Items = new List<Item>()
				};
			}
			else
			{
				widget = new UserWidget()
				{
					WidgetType = widgetType,
					Title = title,
					RowNumber = rowNbr,
					ColumnNumber = colNbr,
					RowSpan = rowspan,
					ColumnSpan = colspan,
					Items = new List<Item>()
				};
			}

			//repo autmaticly links widget to dashboard
			widgetRepo.CreateWidget(widget, dashboardId);
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
			Item itemToAdd = itemManager.GetItem(itemId);

			//Add item to widget
			Widget widgetToUpdate = GetWidget(widgetId);
			widgetToUpdate.Items.Add(itemToAdd);

			//Update database
			widgetRepo.UpdateWidget(widgetToUpdate);

			return widgetToUpdate;
		}

		/// <summary>
		/// Adds multiple items to a single widget.
		/// 
		/// WARNING
		/// THIS METHOD USES UNIT OF WORK
		/// </summary>
		public Widget AddItemsToWidget(int widgetId, IEnumerable<int> itemIds)
		{
			uowManager = new UnitOfWorkManager();
			InitRepo();

			//Get Items
			ItemManager itemManager = new ItemManager(uowManager);
			List<Item> items = new List<Item>();
			foreach (int id in itemIds) items.Add(itemManager.GetItem(id));

			//Add items to widget
			Widget widgetToUpdate = GetWidget(widgetId);
			foreach (Item item in items) widgetToUpdate.Items.Add(item);

			//Update database
			widgetRepo.UpdateWidget(widgetToUpdate);

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
		/// Gives back a dashboard with their widgets.
		/// </summary>
		public Dashboard GetDashboard(int dashboardId)
		{
			InitRepo();
			return widgetRepo.ReadDashboardWithWidgets(dashboardId);
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
			widgetRepo.UpdateDashboard(dashboard);
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
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present.
		/// </summary>
		private void InitRepo()
		{
			if (uowManager == null) widgetRepo = new WidgetRepository();
			else widgetRepo = new WidgetRepository(uowManager.UnitOfWork);
		}
	}
}