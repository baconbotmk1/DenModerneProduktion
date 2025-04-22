using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Models
{
	public class DeviceEvent : BaseModel
    {
        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? Desc { get; set; }

        [Required]
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        [Required]
        public int TypeId { get; set; }
		public DeviceEventType Type { get; set; }

		public DeviceEvent()
		{
		}
	}
}

