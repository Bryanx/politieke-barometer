using BAR.BL.Domain.Items;
using BAR.BL.Domain.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Managers
{
	public interface IWidgetManager
	{
		//widgets
		Widget GetWidget(int widgetId);
		IEnumerable<UserWidget> GetWidgetsForDashboard(int dashboardId);

		Widget CreateWidget(WidgetType widgetType, string title, int rowNbr, int colNbr, int dashboardId = -1, int rowspan = 1, int colspan = 1);
		Widget AddItemToWidget(int widgetId, int itemId);
		Widget AddItemsToWidget(int widgetId, IEnumerable<int> itemIds);

		Widget ChangeWidgetPos(int widgetId, int rowNbr, int colNbr, int rowspan = 1, int colspan = 1);
		Widget ChangeWidgetTitle(int widgetId, string title);

		void RemoveWidget(int widgetId);

		//dashboards
		Dashboard GetDashboard(int dashboardId);

		Dashboard CreateDashboard(string userId, DashboardType dashType = DashboardType.Public);

		void RemoveDashboard(int dashboardId);
	}
}
