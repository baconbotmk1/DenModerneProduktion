using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
	public class Section : BaseModel
    {
        public string Name { get; set; }

		[InverseProperty("Section")]
		public ICollection<Room> Rooms { get; } = new List<Room>();

		public int BuildingId { get; set; }
		public Building Building { get; set; }

		public Section()
		{
		}
	}
}

