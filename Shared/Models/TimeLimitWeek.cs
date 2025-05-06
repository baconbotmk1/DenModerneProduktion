using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

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

        public int GetWeekNumber() => ISOWeek.GetWeekOfYear(new DateTime(StartDate.Value, new TimeOnly(12, 0)));


        public TimeLimitWeek()
		{
		}


        public bool IsActive(DateTime time)
        {
            int currentWeek = ISOWeek.GetWeekOfYear(time);

            if (StartDate != null && GetWeekNumber() != currentWeek)
                return false;


            foreach (TimeLimitWeekDay dayObj in Days)
            {
                if (dayObj.IsActive(time))
                    return true;
            }

            return false;
        }
    }
}

