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

		[InverseProperty("Device")]
        public ICollection<DeviceInfo> Infos { get; } = new List<DeviceInfo>();
        [InverseProperty("Device")]
        public ICollection<DeviceData> Data { get; } = new List<DeviceData>();
        [InverseProperty("Device")]
        public ICollection<DeviceEvent> Events { get; } = new List<DeviceEvent>();

        public Device()
		{
		}
	}
}

