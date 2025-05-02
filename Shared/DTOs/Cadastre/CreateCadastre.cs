using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.Cadastre
{
	public class CreateCadastre
	{
        [Required]
        public required string Name { get; set; }
    }
}

