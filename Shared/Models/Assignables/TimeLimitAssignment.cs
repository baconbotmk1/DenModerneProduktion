using System;
namespace Shared.Models.Assignables
{
	public abstract class TimeLimitAssignment : BaseModel
	{
        public int TimeLimitId { get; set; }
        public TimeLimit TimeLimit { get; set; }
	}
}

