using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
	public abstract class TimeLimitableModel : BaseModel
	{
		public int? TimeLimitId { get; set; }
		public TimeLimit? TimeLimit { get; set; }

		public TimeLimitableModel()
		{
		}
	}
}

