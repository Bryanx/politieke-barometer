using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Widgets
{
	public class WidgetData
	{
		public int WidgetDataId { get; set; }
		public string KeyValue { get; set; }
		public ICollection<GraphValue> GraphValues { get; set; }
		public Widget Widget { get; set; }

		public WidgetData() {
		}

		//Constructor for making a copy of WidgetData
		public WidgetData(WidgetData widgetData) {
			KeyValue = widgetData.KeyValue;
			GraphValues = widgetData.GraphValues;
		}
	}
}
