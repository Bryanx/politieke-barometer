using System;

namespace BAR.BL.Domain.Users
{
	public abstract class Alert
	{
		public int AlertId { get; set; }
		public string Subject { get; set; }
		public string Message { get; set; }
		public AlertType AlertType { get; set; }
		public bool IsRead { get; set; }
		public DateTime? TimeStamp { get; set; }
	}
}
