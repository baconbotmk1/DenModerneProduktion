using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Models
{
    public class DeviceDataLimitValue : BaseModel
    {
        public double? MinimumLimit { get; set; }
        public double? MaximumLimit { get; set; }
        public int TypeId { get; set; }
        public DeviceDataType Type { get; set; }
        
        public int? RoomId { get; set; }
        public Room? Room { get; set; }
        public int? SectionId { get; set; }
        public Section? Section { get; set; }
        public int? BuildingId { get; set; }
        public Building? Building { get; set; }
        public int? CadastreId { get; set; }
        public Cadastre? Cadastre { get; set; }


        public DeviceDataLimitValue()
        {
        }
    }
}

