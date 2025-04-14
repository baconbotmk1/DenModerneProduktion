using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
	public class Section : BaseModel
    {
        [Required]
        public string Name { get; set; } = default!;

        [InverseProperty("Section")]
		public ICollection<Room> Rooms { get; } = new List<Room>();

        [Required]
        public int BuildingId { get; set; } = default!;
        public Building Building { get; set; } = default!;
	}
}

