using BAR.BL.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Items
{
	public class Item
	{
		public int ItemId { get; set; }
		public SubPlatform SubPlatform { get; set; }
		public string Name { get; set; }
		public DateTime CreationDate { get; set; }
		public DateTime? LastUpdated { get; set; }
		public string Description { get; set; }
		public int NumberOfFollowers { get; set; }
		public double TrendingPercentage { get; set; }
		public int NumberOfMentions { get; set; }
		public double Baseline { get; set; }
		public ICollection<Information> Informations { get; set; }
	}
}
