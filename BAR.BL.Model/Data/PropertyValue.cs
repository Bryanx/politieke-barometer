namespace BAR.BL.Domain.Data
{
	public class PropertyValue
	{
		public int PropertyValueId { get; set; }
		public Property Property { get; set; }
		public string Value { get; set; }
		public double Confidence { get; set; }
	}
}
