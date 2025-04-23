using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.DeviceEvent
{
	public class CreateDeviceEvent
	{
        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Required]
        public int DeviceId { get; set; }

        [Required]
        public int TypeId { get; set; }
    }
}

