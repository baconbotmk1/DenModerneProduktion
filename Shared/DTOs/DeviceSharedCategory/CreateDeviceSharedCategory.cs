using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.DeviceSharedCategory
{
	public class CreateDeviceSharedCategory
    {
        [Required]
        public string Name { get; set; }

        public string Desc { get; set; }
    }
}

