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

        private static Dictionary<DayOfWeek, Days> RealDayOfWeek = new Dictionary<DayOfWeek, Days>()
        {
            { DayOfWeek.Monday, Days.MONDAY },
            { DayOfWeek.Tuesday, Days.TUESDAY },
            { DayOfWeek.Wednesday, Days.WEDNESDAY },
            { DayOfWeek.Thursday, Days.THURSDAY },
            { DayOfWeek.Friday, Days.FRIDAY },
            { DayOfWeek.Saturday, Days.SATURDAY },
            { DayOfWeek.Sunday, Days.SUNDAY }
        };

        public bool IsActive(DateTime time)
        {
            Days currentDay = RealDayOfWeek[time.DayOfWeek];

            if (Day != currentDay)
                return false;

            foreach (TimeLimitWeekDayTime timeObj in Times)
            {
                if (timeObj.IsActive(time))
                    return true;
            }

            return false;
        }
    }
}

