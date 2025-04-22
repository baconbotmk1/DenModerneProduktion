using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.DeviceType
{
	public class CreateDeviceType
	{
        [Required]
        public string Name { get; set; }
        public string? Desc { get; set; }
    }
}

