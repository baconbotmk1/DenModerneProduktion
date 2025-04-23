using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Device
{
	public class CreateDevice
	{
        [Required]
        public string Name { get; set; }

        public string? Identifier { get; set; }

        [Required]
        public int TypeId { get; set; }

        public int? RoomId { get; set; }
        public int? SectionId { get; set; }
    }
}

