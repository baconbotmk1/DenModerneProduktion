using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
	public class TimeLimitWeek : BaseModel
    {
		public string Name { get; set; } = "";

		public DateOnly? StartDate { get; set; }

        [InverseProperty("Week")]
        public ICollection<TimeLimitWeekDay> Days { get; set; } = new List<TimeLimitWeekDay>();

		public int TimeLimitId { get; set; }
		public TimeLimit TimeLimit { get; set; }

		public TimeLimitWeek()
		{
		}
	}
}

