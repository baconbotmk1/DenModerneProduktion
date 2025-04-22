using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.DeviceInfoType
{
	public class CreateDeviceInfoType
	{
        [Required]
        public string Type { get; set; }

        public string? Desc { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}

