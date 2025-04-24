using System;
namespace Shared.Models.Assignables.TimeLimit
{
	public class SecurityGroupSectionTL : TimeLimitAssignment
	{
        public int SecurityGroupSectionId { get; set; }
        public SecurityGroupSection SecurityGroupSection { get; set; }
	}
}

