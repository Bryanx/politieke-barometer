﻿using BAR.BL.Domain.Items;
using BAR.BL.Domain.Widgets;
using System;
using System.Collections.Generic;
using BAR.BL.Domain.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Managers
{
	public interface IWidgetManager
	{
		//widgets
		Widget GetWidget(int widgetId);
		Widget GetWidgetWithAllItems(int widgetId);
		Widget GetWidgetWithAllData(int widgetId);
		IEnumerable<UserWidget> GetWidgetsForDashboard(int dashboardId);
		IEnumerable<Widget> GetAllWidgetsWithAllItems();
		IEnumerable<Widget> GetAllWidgetsWithAllData();
		IEnumerable<Widget> GetWidgetsForItem(int itemId);
		Widget AddWidget(WidgetType widgetType, string title, int rowNbr, int colNbr, List<PropertyTag> proptags,
			DateTime? timestamp = null, GraphType? graphType = null, int rowspan = 1, int colspan = 1, int dashboardId = -1);
		Widget AddItemToWidget(int widgetId, int itemId);

		Widget ChangeWidgetPos(int widgetId, int rowNbr, int colNbr, int rowspan = 1, int colspan = 1);
		Widget ChangeWidgetTitle(int widgetId, string title);
		Widget ChangeWidget(Widget widget);
		IEnumerable<Widget> ChangeWidgets(IEnumerable<Widget> widgets);

		void RemoveWidget(int widgetId);

		//dashboards
		Dashboard GetDashboard(int dashboardId);
		Dashboard GetDashboard(string userId);

		Dashboard AddDashboard(string userId, DashboardType dashType = DashboardType.Private);

		void RemoveDashboard(int dashboardId);
	}
}
