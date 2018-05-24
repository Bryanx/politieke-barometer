using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;

namespace BAR.BL.Domain.Items
{
	public class Person : Item
	{
		public string District { get; set; }
		public string Level { get; set; }
		public Gender Gender { get; set; }
		public string Site { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public Area Area { get; set; }
		public string Position { get; set; }
		public Organisation Organisation { get; set; }
		public List<SocialMediaName> SocialMediaNames { get; set; }
	}
}
