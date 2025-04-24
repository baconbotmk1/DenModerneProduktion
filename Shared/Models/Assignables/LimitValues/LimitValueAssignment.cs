using System;
namespace Shared.Models.Assignables.LimitValues
{
    public abstract class LimitValueAssignment : BaseModel
    {
        public int LimitValueId { get; set; }
        public DeviceDataLimitValue LimitValue { get; set; }
    }
}

