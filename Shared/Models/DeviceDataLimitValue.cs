using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Models
{
    public class DeviceDataLimitValue : BaseModel
    {
        public double MinimumLimit { get; set; }
        public double MaximumLimit { get; set; }
        public int TypeId { get; set; }
        public DeviceDataType Type { get; set; }


        public DeviceDataLimitValue()
        {
        }
    }
}

