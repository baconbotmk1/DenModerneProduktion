using System;
namespace Shared.Models.Assignables.TimeLimit
{
	public abstract class TimeLimitAssignment : BaseModel
	{
        public int TimeLimitId { get; set; }
        public Shared.Models.TimeLimit TimeLimit { get; set; }
	}
}

