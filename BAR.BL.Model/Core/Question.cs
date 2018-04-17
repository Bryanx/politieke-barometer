using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Users
{
	public class Question
	{
		public int QuestionId { get; set; }
		public QuestionType QuestionType { get; set; }
		public string Title { get; set; }
		public string Answer { get; set; }
	}
}k
