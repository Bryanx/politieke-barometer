using BAR.BL.Domain.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Managers
{
	public interface IDashboardManager
	{
		Widget GetWidget(int widgetId);
		IEnumerable<Widget> GetWidgets(int dashboardId);
		Widget CreateWidget(int dashboardId, string title, int rowNbr, int colNbr, int rowspan, int colspan);
	}
}
