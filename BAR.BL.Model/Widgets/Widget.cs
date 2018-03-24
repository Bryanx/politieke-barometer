using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Widgets
{
	public class Widget
	{
		public int WidgetId { get; set; }
		public string Title { get; set; }
		public int ColumnSpan { get; set; }
		public int RowSpan { get; set; }
		public int ColumnNumber { get; set; }
		public int RowNumber { get; set; }
		public WidgetType WidgetType { get; set; }
		public Dashboard Dashboard { get; set; }
	}
}
