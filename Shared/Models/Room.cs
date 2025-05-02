using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Models
{
    public class Room : BaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int SectionId { get; set; }
        public Section Section { get; set; }
        public ICollection<DeviceDataLimitValue> LimitValues { get; set; } = new List<DeviceDataLimitValue>();
        public ICollection<Device> Devices { get; set; } = new List<Device>();
        public ICollection<TimeLimit> TimeLimits { get; set; } = new List<TimeLimit>();


        public Room()
        {
        }
    }
}

