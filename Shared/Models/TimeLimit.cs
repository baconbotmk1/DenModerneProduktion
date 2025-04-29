using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
	public class TimeLimit : BaseModel
    {
        [Required]
        public string Name { get; set; }

		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }

		public ICollection<TimeLimitWeek> Weeks { get; set; } = new List<TimeLimitWeek>();

        public TimeLimit()
		{
		}
	}
}

