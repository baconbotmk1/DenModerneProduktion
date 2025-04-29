using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
	public class TimeLimitWeekDay : BaseModel
    {
		public Days Day { get; set; }

		[InverseProperty("WeekDay")]
		public ICollection<TimeLimitWeekDayTime> Times { get; set; } = new List<TimeLimitWeekDayTime>();

		public int WeekId { get; set; }
		public TimeLimitWeek Week { get; set; }

		public TimeLimitWeekDay()
		{
		}
	}
}

