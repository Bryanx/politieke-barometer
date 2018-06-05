using BAR.BL.Domain.Items;
using BAR.BL.Domain.Widgets;
using System;
using System.Collections.Generic;
using BAR.BL.Domain.Data;

namespace BAR.BL.Managers
{
	public interface IWidgetManager
	{
		//widgets
		Widget GetWidget(int widgetId);
		Widget GetWidgetWithAllItems(int widgetId);
		Widget GetWidgetWithAllData(int widgetId);
		Widget GetGeoLocationWidget();
		IEnumerable<Widget> GetItemwidgetsForItem(int itemId);
		IEnumerable<Widget> GetAllWidgetsWithAllDataForItem(int itemId);
		IEnumerable<Widget> GetWidgetsForWeeklyReview(int platformId, string userId = null);
		IEnumerable<Widget> GetWidgetsForActivities(int platformId);
		IEnumerable<UserWidget> GetWidgetsForDashboard(int dashboardId);

		Widget AddWidget(WidgetType widgetType, string title, int rowNbr, int colNbr, List<PropertyTag> proptags,
			DateTime? timestamp = null, GraphType? graphType = null, int rowspan = 1, int colspan = 1, int dashboardId = -1, List<WidgetData> datas = null, List<Item> items = null);

		Widget ChangeWidgetDetails(int widgetId, int rowNbr, int colNbr, List<int> itemIds, int rowspan = 1, int colspan = 1, GraphType graphType = 0);
		Widget ChangeWidgetTitle(int widgetId, string title);
		Widget ChangeWidget(Widget widget);
		IEnumerable<Widget> ChangeWidgetActities(IEnumerable<Widget> widgets, int platformId);

		void RemoveWidget(int widgetId);

		void GenerateDataForPersonsAndThemes();
		void GenerateDataForOrganisations();
		void MoveWidgetToDashBoard(int widgetId, GraphType graphType, IEnumerable<int> itemIds, string userId);

		//dashboards
		Dashboard GetDashboard(string userId);
		Dashboard GetDashboardWithAllDataForUserId(string userId);

		Dashboard AddDashboard(string userId, DashboardType dashType = DashboardType.Private);

		//WidgetDatas
		IEnumerable<WidgetData> GetAllWidgetDatas();
		IEnumerable<WidgetData> GetWidgetDatasForItemId(int itemId);
		IEnumerable<WidgetData> GetWidgetDatasForKeyvalue(string value);
		IEnumerable<WidgetData> GetWidgetDatasForWidgetId(int widgetId);

		WidgetData AddWidgetData(WidgetData widgetData);

		void RemoveWidgetDatas(IEnumerable<WidgetData> datas);

		string NoteboxData(int itemId);
	}
}
