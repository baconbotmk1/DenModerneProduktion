using System;
namespace Shared.Models.Assignables
{
	public class SecurityGroupSectionTL : TimeLimitAssignment
	{
        public int SecurityGroupSectionId { get; set; }
        public SecurityGroupSection SecurityGroupSection { get; set; }
	}
}

