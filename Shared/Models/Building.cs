﻿using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Models
{
	public class Building : BaseModel
    {
        [Required]
        public int CadastreId { get; set; }
        [Required]
        public string Name { get; set; }
        public Cadastre Cadastre { get; set; }

		public ICollection<Section> Sections { get; set; } = new List<Section>();
        public ICollection<Room> Rooms { get; set; } = new List<Room>(); 
        public ICollection<DeviceDataLimitValue> LimitValues { get; set; } = new List<DeviceDataLimitValue>();




        public Building()
		{
		}
	}
}

