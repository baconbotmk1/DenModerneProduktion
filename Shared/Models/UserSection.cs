using System;
namespace Shared.Models
{
	public class UserSection : TimeLimitableModel
    {
		public int UserId { get; set; }
		public User User { get; set; }
		public int SectionId { get; set; }
		public Section Section { get; set; }

        public ICollection<TimeLimit> TimeLimits { get; set; } = new List<TimeLimit>();

        public UserSection()
		{
		}
	}
}

