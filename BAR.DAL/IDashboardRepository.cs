﻿using BAR.BL.Domain.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.DAL
{
	public interface IDashboardRepository
	{
		//Read
		Dashboard ReadDashboard(int dashboardId);
		Dashboard ReadGeneralDashboard();
		IEnumerable<Dashboard> ReadAllDashboards();
		Widget ReadWidget(int widgetId);
		IEnumerable<Widget> ReadAllWidgets();
		IEnumerable<Widget> ReadWidgetsForDashboard(int dashboardId);

		//Create
		int CreateDashboard(Dashboard dashboard);
		int CreateWidget(Widget widget, int dashboardId);

		//Update
		//...

		//Delete
		//...
	}
}
