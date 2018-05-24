using BAR.BL.Domain.Items;
using System;
using System.Collections.Generic;
using BAR.BL.Domain.Data;

namespace BAR.BL.Domain.Widgets
{
	public abstract class Widget
	{
		public int WidgetId { get; set; }
		public string Title { get; set; }
		public int ColumnSpan { get; set; }
		public int RowSpan { get; set; }
		public int ColumnNumber { get; set; }
		public int RowNumber { get; set; }
		public ICollection<PropertyTag> PropertyTags { get; set; }
		public DateTime? Timestamp { get; set; }
		public WidgetType WidgetType { get; set; }
		public ICollection<Item> Items { get; set; }
		public GraphType? GraphType { get; set; }
		public ICollection<WidgetData> WidgetDatas { get; set; }
	}
}
