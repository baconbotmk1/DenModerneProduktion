using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
	public abstract class BaseModel
	{
		[Key]
		public int Id { get; set; }

		public BaseModel()
		{
		}
	}
}

