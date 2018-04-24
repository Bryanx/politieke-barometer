﻿using BAR.BL.Domain.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.DAL
{
	public interface IWidgetRepository
	{
		//Read
		Dashboard ReadDashboard(int dashboardId);
		Dashboard ReadDashboardWithWidgets(int dashboardId);
		Dashboard ReadDashboardWithWidgets(string userId);
		Dashboard ReadGeneralDashboard();
		IEnumerable<Dashboard> ReadAllDashboards();
		Widget ReadWidget(int widgetId);
		Widget ReadWidgetWithAllitems(int widgetid);
		IEnumerable<Widget> ReadAllWidgets();
		IEnumerable<UserWidget> ReadWidgetsForDashboard(int dashboardId);

		//Create
		int CreateDashboard(Dashboard dashboard);
		int CreateWidget(Widget widget, int dashboardId);

		//Update
		int UpdateDashboard(Dashboard dashboard);
		int UpdateDashboards(IEnumerable<Dashboard> dashboards);
		int UpdateWidget(Widget widget);
		int UpdateWidgets(IEnumerable<Widget> widgets);

		//Delete
		int DeleteDashboard(int dashboardId);
		int DeleteDashboards(IEnumerable<int> dashboardIds);
		int DeleteWidget(Widget widgetId);
		int DeleteWidgets(IEnumerable<Widget> widgets);
	}
}
