using BAR.BL.Domain.Widgets;
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
		Dashboard ReadDashboardWithWidgets(int dashboardId);
		Dashboard ReadGeneralDashboard();
		IEnumerable<Dashboard> ReadAllDashboards();
		UserWidget ReadWidget(int widgetId);
		IEnumerable<UserWidget> ReadAllWidgets();
		IEnumerable<UserWidget> ReadWidgetsForDashboard(int dashboardId);

		//Create
		int CreateDashboard(Dashboard dashboard);
		int CreateWidget(UserWidget widget, int dashboardId);

		//Update
		int UpdateDashboard(Dashboard dashboard);
		int UpdateDashboards(IEnumerable<Dashboard> dashboards);
		int UpdateWidget(UserWidget widget);
		int UpdateWidgets(IEnumerable<UserWidget> widgets);

		//Delete
		int DeleteDashboard(int dashboardId);
		int DeleteDashboards(IEnumerable<int> dashboardIds);
		int DeleteWidget(UserWidget widgetId);
		int DeleteWidgets(IEnumerable<UserWidget> widgets);
	}
}
