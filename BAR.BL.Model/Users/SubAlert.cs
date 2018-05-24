namespace BAR.BL.Domain.Users
{
	public class SubAlert : Alert
	{
		public Subscription Subscription { get; set; }
		public bool IsSend { get; set; }
	}
}
