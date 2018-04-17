﻿using BAR.BL.Domain.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Managers
{
	public interface IDashboardManager
	{
		//widgets
		Widget GetWidget(int widgetId);
		IEnumerable<Widget> GetWidgets(int dashboardId);

		Widget CreateWidget(int dashboardId, WidgetType widgetType, string title, int rowNbr, int colNbr, int rowspan = 1, int colspan = 1);

		Widget ChangeWidgetPos(int widgetId, int rowNbr, int colNbr, int rowspan = 1, int colspan = 1);
		Widget ChangeWidgetTitle(int widgetId, string title);

		void RemoveWidget(int widgetId);

		//dashboards
		Dashboard GetDashboard(int dashboardId);

		Dashboard CreateDashboard(string userId, DashboardType dashType);

		void RemoveDashboard(int dashboardId);
	}
}