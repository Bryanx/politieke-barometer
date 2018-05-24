using System.Collections.Generic;

namespace BAR.BL.Domain.Items
{
	public class Theme : Item
	{
		public ICollection<Keyword> Keywords { get; set; }
	}
}
