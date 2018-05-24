using System.Collections.Generic;

namespace BAR.BL.Domain.Items
{
	public class Organisation : Item
	{
		public string Site { get; set; }
		public List<SocialMediaName> SocialMediaUrls { get; set; }
	}
}
