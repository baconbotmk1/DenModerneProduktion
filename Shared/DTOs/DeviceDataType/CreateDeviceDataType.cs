using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.DeviceDataType
{
	public class CreateDeviceDataType
	{
        [Required]
        public string Name { get; set; }
        [Required]
        public string DataType { get; set; }

        public string? Desc { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}

