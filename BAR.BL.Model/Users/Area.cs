using System.Collections.Generic;

namespace BAR.BL.Domain.Users
{
	public class Area
	{
		public int AreaId { get; set; }
		public string Residence { get; set; }
		public string PostalCode { get; set; }
		public string Country { get; set; }
		public ICollection<User> Users { get; set; }
	}
}
