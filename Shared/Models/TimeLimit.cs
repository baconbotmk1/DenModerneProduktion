using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shared.Models.Assignables.TimeLimit;

namespace Shared.Models
{
	public class TimeLimit : BaseModel
    {
        [Required]
        public string Name { get; set; }

		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }

		public ICollection<TimeLimitWeek> Weeks { get; set; } = new List<TimeLimitWeek>();

        public ICollection<TimeLimitAssignment> Assignments { get; set; } = new List<TimeLimitAssignment>();

        public TimeLimit()
		{
		}
	}
}

