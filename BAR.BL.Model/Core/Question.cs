namespace BAR.BL.Domain.Core
{
	public class Question
	{
		public int QuestionId { get; set; }
		public QuestionType QuestionType { get; set; }
		public string Title { get; set; }
		public string Answer { get; set; }
		public SubPlatform SubPlatform { get; set; }
	}
}
