using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.DeviceInfo
{
	public class CreateDeviceInfo
	{
        [Required]
        public string Value { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Required]
        public int DeviceId { get; set; }

        [Required]
        public int TypeId { get; set; }
    }
}

