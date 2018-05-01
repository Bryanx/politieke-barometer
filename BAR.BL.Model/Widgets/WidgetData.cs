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
	}
}
