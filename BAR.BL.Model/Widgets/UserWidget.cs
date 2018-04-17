using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Widgets
{
	public class UserWidget : Widget
	{
		public WidgetType WidgetType { get; set; }
		public Dashboard Dashboard { get; set; }
	}
}
