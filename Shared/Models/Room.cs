using System;
namespace Shared.Models
{
	public class Room : BaseModel
	{
		public string Name { get; set; }

		public int SectionId { get; set; }
		public Section Section { get; set; }

		public Room()
		{
		}
	}
}

