using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
	public class Device : BaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int TypeId { get; set; }
		public DeviceType Type { get; set; }

        public int? RoomId { get; set; }
        public Room? Room { get; set; }

        public int? SectionId { get; set; }
        public Section? Section { get; set; }

        public string? Identifier { get; set; }

        [InverseProperty("Device")]
        public ICollection<DeviceInfo> Infos { get; set; } = new List<DeviceInfo>();
        [InverseProperty("Device")]
        public ICollection<DeviceData> Data { get; set; } = new List<DeviceData>();
        [InverseProperty("Device")]
        public ICollection<DeviceEvent> Events { get; set; } = new List<DeviceEvent>();

        public Device()
		{
		}
	}
}

