﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
	public abstract class BaseModel
	{
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public BaseModel()
		{
		}

		public virtual void LoadReferences() { }


		public BaseModel AddReferences()
		{
			LoadReferences();

            return this;
		}

    }
}

