using System;
namespace Shared.Models
{
	public class TimeLimitWeekDayTime : BaseModel
    {
		public DateTime FromTime { get; set; }
		public DateTime ToTime { get; set; }

		public int WeekDayId { get; set; }
		public TimeLimitWeekDay WeekDay { get; set; }

		public TimeLimitWeekDayTime()
		{
		}
	}
}

