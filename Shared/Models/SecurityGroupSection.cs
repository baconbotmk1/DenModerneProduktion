using Shared.Models.Assignables.TimeLimit;
using System;
namespace Shared.Models
{
	public class SecurityGroupSection : TimeLimitableModel
    {
		public int SecurityGroupId { get; set; }
		public SecurityGroup SecurityGroup { get; set; }
		public int SectionId { get; set; }
		public Section Section { get; set; }

        public ICollection<SecurityGroupSectionTL> TimeLimits { get; set; } = new List<SecurityGroupSectionTL>();

        public SecurityGroupSection()
		{
		}
	}
}

