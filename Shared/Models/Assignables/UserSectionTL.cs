using System;
namespace Shared.Models.Assignables
{
	public class UserSectionTL : TimeLimitAssignment
	{
        public int UserSectionId { get; set; }
        public UserSection UserSection { get; set; }
	}
}

