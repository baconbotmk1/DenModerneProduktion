using System;
using System.Collections.Generic;
using Shared.Models.Assignables;

namespace Shared.Models
{
	public class TimeLimit : BaseModel
    {
		public string Name { get; set; }

		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }

		public ICollection<TimeLimitWeek> Weeks { get; } = new List<TimeLimitWeek>();

        public ICollection<TimeLimitAssignment> Assignments { get; set; } = new List<TimeLimitAssignment>();

        public TimeLimit()
		{
		}
	}
}

