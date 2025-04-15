using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs
{
	public class CreateSection
	{
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public int BuildingId { get; set; } = default!;
    }
}

