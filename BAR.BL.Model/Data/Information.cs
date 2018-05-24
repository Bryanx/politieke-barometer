using BAR.BL.Domain.Items;
using System;
using System.Collections.Generic;

namespace BAR.BL.Domain.Data
{
	public class Information
	{
		public int InformationId { get; set; }
		public ICollection<Item> Items { get; set; }
		public Source Source { get; set; }
		public DateTime? CreationDate { get; set; }
		public ICollection<PropertyValue> PropertieValues { get; set; }
	}
}
