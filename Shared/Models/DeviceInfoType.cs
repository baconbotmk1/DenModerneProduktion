﻿using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Models
{
	public class DeviceInfoType : BaseModel
    {
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string DataType { get; set; }

        public string? Desc { get; set; }
        public string? Suffix { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public DeviceSharedCategory Category { get; set; }

        public string GetSlug() => Category.Name + " - " + Name;

        public DeviceInfoType()
		{
		}
	}
}

