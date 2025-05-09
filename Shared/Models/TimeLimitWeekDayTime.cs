﻿using System;
namespace Shared.Models
{
	public class TimeLimitWeekDayTime : BaseModel
    {
		public TimeOnly FromTime { get; set; }
		public TimeOnly ToTime { get; set; }

		public int WeekDayId { get; set; }
		public TimeLimitWeekDay WeekDay { get; set; }

		public TimeLimitWeekDayTime()
		{
		}

        public bool IsActive( DateTime time )
        {
            TimeOnly now = TimeOnly.FromDateTime(time);
            if (now < FromTime)
                return false;
            if (now > ToTime)
                return false;

            return true;
        }
    }
}

