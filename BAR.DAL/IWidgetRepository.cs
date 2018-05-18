using BAR.BL.Domain.Items;
using BAR.BL.Domain.Widgets;
using System.Collections.Generic;

namespace BAR.DAL
{
	public interface IWidgetRepository
	{
		//Read
		Dashboard ReadDashboardWithWidgets(int dashboardId);
		Dashboard ReadDashboardWithWidgets(string userId);
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
		IEnumerable<Widget> ReadActivityWidgets();
		IEnumerable<Widget> ReadWidgetsForItemtype(ItemType type);
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
		int UpdateWidget(Widget widget);
		int UpdateWidgets(IEnumerable<Widget> widgets);
		int UpdateWidgetData(WidgetData widgetData);

		//Delete
		int DeleteWidget(Widget widgetId);
		int DeleteWidgetDatas(IEnumerable<WidgetData> datas);
	}
}
