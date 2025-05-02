using Shared.Models;

namespace DenModerneProduktion.Components.Pages.PageModels
{
    public class DeviceDisplayData
    {
        public string RoomName { get; set; }
        public double? MaxLimit { get; set; }
        public double? MinLimit { get; set; }
        public List<DeviceInfo>? _Info { get; set; }
        public List<DeviceData>? _Data { get; set; }
    }
}
