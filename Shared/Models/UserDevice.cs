﻿using System;
namespace Shared.Models
{
	/// <summary>
	/// A User is given access to a door through it's Device.
	/// </summary>
	public class UserDevice : BaseModel
    {
		public int UserId { get; set; }
		public User User { get; set; }
		public int DeviceId { get; set; }
		public Device Device { get; set; }

        public ICollection<TimeLimit> TimeLimits { get; set; } = new List<TimeLimit>();

        public UserDevice()
		{
		}
	}
}

