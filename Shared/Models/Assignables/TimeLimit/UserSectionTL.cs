using System;
namespace Shared.Models.Assignables.TimeLimit
{
	public class UserSectionTL : TimeLimitAssignment
	{
        public int UserSectionId { get; set; }
        public UserSection UserSection { get; set; }
	}
}

