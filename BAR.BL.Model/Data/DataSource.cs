using System;

namespace BAR.BL.Domain.Data
{
	public class DataSource
	{
		public int DataSourceId { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
		//The interval of the timer in minutes.
		public int Interval { get; set; }
		//When the timer starts running every day.
		public string SetTime { get; set; }
		public DateTime LastTimeChecked { get; set; }
	}
}
