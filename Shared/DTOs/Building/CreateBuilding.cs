using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.Building
{
	public class CreateBuilding
	{
        [Required]
        public required string Name { get; set; }

        [Required]
        public required int CadastreId { get; set; }
    }
}

