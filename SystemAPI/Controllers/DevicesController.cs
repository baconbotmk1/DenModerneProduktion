using Shared.DTOs.Device;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DevicesController(DataContext _context, IConfiguration _configuration, IServiceProvider _provider) : BaseController(_context, _configuration, _provider)
    {
        /// <summary>
        /// Get all devices
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<Device>> Get()
        {
            var data = context.Devices
                .Include(e => e.Type)
                .Include(e => e.Section)
                .Include(e => e.Room)
                .Include(e => e.Infos)
                .Include(e => e.Data)
                .Include(e => e.Events)
                .ToList();

            return Ok(data);
        }

        /// <summary>
        /// Create a new device
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Device> Post([FromBody] CreateDevice data)
        {
            Device item = data.Adapt<Device>();

            context.Devices.Add(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Get a device by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Device> GetById(int id)
        {
            Device? device = context.Devices
                .AsNoTracking()
                .Include(e => e.Type)
                .Include(e => e.Section)
                .Include(e => e.Room)
                .Include(e => e.Infos)
                    .ThenInclude(e => e.Type)
                        .ThenInclude(e => e.Category)
                .Include(e => e.Data)
                    .ThenInclude(e => e.Type)
                        .ThenInclude(e => e.Category)
                .Include(e => e.Events)
                    .ThenInclude(e => e.Type)
                        .ThenInclude(e => e.Category)
                .FirstOrDefault(e => e.Id == id);

            if (device == null)
            {
                return NotFound();
            }

            return Ok(device);
        }

        /// <summary>
        /// Update a device
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<Device> Put(int id, [FromBody] CreateDevice data)
        {
            Device? item = context.Devices.FirstOrDefault(e => e.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            data.Adapt(item);

            context.Devices.Update(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Delete a device
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Device? foundObject = context.Devices.FirstOrDefault(e => e.Id == id);

            if (foundObject == null)
            {
                return NotFound();
            }

            context.Devices.Remove(foundObject);
            context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Get all device info
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("{id}/info")]
        public ActionResult<DeviceInfo> PostInfo(int id, [FromBody] Shared.DTOs.DeviceInfo.CreateDeviceInfo data)
        {
            DeviceInfo item = data.Adapt<DeviceInfo>();

            DeviceInfo? itemOrNull = context.DeviceInfos.FirstOrDefault(e => e.DeviceId == item.DeviceId && e.TypeId == item.TypeId);

            if(itemOrNull != null)
            {
                data.Adapt(itemOrNull);

                context.Attach(itemOrNull);
                context.SaveChanges();
            }
            else
            {
                itemOrNull = item;
                context.DeviceInfos.Add(item);
                context.SaveChanges();
            }

            return Ok(itemOrNull);
        }

        /// <summary>
        /// Get all device data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("{id}/data")]
        public ActionResult<DeviceData> PostData(int id, [FromBody] Shared.DTOs.DeviceData.CreateDeviceData data)
        {
            DeviceData item = data.Adapt<DeviceData>();

            context.DeviceDatas.Add(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Get all device events
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("{id}/event")]
        public ActionResult<DeviceEvent> PostEvent(int id, [FromBody] Shared.DTOs.DeviceEvent.CreateDeviceEvent data)
        {
            DeviceEvent item = data.Adapt<DeviceEvent>();

            context.DeviceEvents.Add(item);
            context.SaveChanges();

            return Ok(item);
        }
    }
}

