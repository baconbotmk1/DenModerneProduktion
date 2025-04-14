using System;
namespace Shared.Models
{
	/// <summary>
	/// A User is given access to a door through it's Device.
	/// </summary>
	public class UserDevice : TimeLimitableModel
    {
		public int UserId { get; set; }
		public User User { get; set; }
		public int DeviceId { get; set; }
		public Device Device { get; set; }

		public UserDevice()
		{
		}
	}
}

