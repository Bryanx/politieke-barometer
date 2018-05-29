namespace BAR.BL.Domain.Widgets
{
	public class GraphValue
	{
		public int GraphValueId { get; set; }
		public string Value { get; set; }
		public double NumberOfTimes { get; set; }

		public GraphValue() { }
		
		//Constructor for making a copy of GraphValue.
		public GraphValue(GraphValue graphValue) {
			Value = graphValue.Value;
			NumberOfTimes = graphValue.NumberOfTimes;
		}
	}
}
