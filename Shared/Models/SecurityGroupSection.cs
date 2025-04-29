using System;
namespace Shared.Models
{
	public class SecurityGroupSection : BaseModel
    {
		public int SecurityGroupId { get; set; }
		public SecurityGroup SecurityGroup { get; set; }
		public int SectionId { get; set; }
		public Section Section { get; set; }

        public ICollection<TimeLimit> TimeLimits { get; set; } = new List<TimeLimit>();

        public SecurityGroupSection()
		{
		}
	}
}

