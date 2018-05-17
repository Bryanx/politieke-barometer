using BAR.BL.Domain.Widgets;
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
		Dashboard ReadDashboardWithAllDataForUserId(string userId);
		IEnumerable<Dashboard> ReadAllDashboards();
		Widget ReadWidget(int widgetId);
		Widget ReadWidgetWithAllitems(int widgetid);
		Widget ReadWidgetWithAllData(int widgetId);
		Widget ReadGeoLocationWidget();
		IEnumerable<Widget> ReadAllWidgets();
		IEnumerable<Widget> ReadAllWidgetsWithAllItems();
		IEnumerable<Widget> ReadAllWidgetsWithAllData();
		IEnumerable<Widget> ReadWidgetsWithAllDataForItem(int itemId);
		IEnumerable<UserWidget> ReadWidgetsForDashboard(int dashboardId);
		IEnumerable<WidgetData> ReadAllWidgetDatas();
		IEnumerable<WidgetData> ReadWidgetDatasForitemid(int itemId);
		IEnumerable<WidgetData> ReadWidgetDatasForKeyvalue(string value);
		
		//Create
		int CreateDashboard(Dashboard dashboard);
		int CreateWidget(Widget widget);
		int CreateWidgetData(WidgetData widgetData);
		int CreateWidgetDatas(ICollection<WidgetData> items);

		//Update
		int UpdateDashboard(Dashboard dashboard);
		int UpdateDashboards(IEnumerable<Dashboard> dashboards);
		int UpdateWidget(Widget widget);
		int UpdateWidgets(IEnumerable<Widget> widgets);
		int UpdateWidgetData(WidgetData widgetData);

		//Delete
		int DeleteDashboard(int dashboardId);
		int DeleteDashboards(IEnumerable<int> dashboardIds);
		int DeleteWidget(Widget widgetId);
		int DeleteWidgets(IEnumerable<Widget> widgets);
		int DeleteWidgetDatas(IEnumerable<WidgetData> datas);
	}
}
