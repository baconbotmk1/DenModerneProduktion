using Shared.DTOs.DeviceEventType;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/device_event_types")]
    public class DeviceEventTypesController(DataContext _context, IConfiguration _configuration, IServiceProvider _provider) : BaseController(_context, _configuration, _provider)
    {
        /// <summary>
        /// Get all device event types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<DeviceEventType>> Get()
        {
            var data = context.DeviceEventTypes
                .AsNoTracking()
                .Include(e => e.Category)
                .ToList();

            return Ok(data);
        }

        /// <summary>
        /// Create a new device event type
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DeviceEventType> Post([FromBody] CreateDeviceEventType data)
        {
            DeviceEventType item = data.Adapt<DeviceEventType>();

            context.DeviceEventTypes.Add(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Get a device event type by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<DeviceEventType> GetById(int id)
        {
            DeviceEventType? deviceDataType = context.DeviceEventTypes.FirstOrDefault(e => e.Id == id);

            if (deviceDataType == null)
            {
                return NotFound();
            }

            return Ok(deviceDataType);
        }

        /// <summary>
        /// Update a device event type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<DeviceEventType> Put(int id, [FromBody] CreateDeviceEventType data)
        {
            DeviceEventType? item = context.DeviceEventTypes.FirstOrDefault(e => e.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            data.Adapt(item);

            context.DeviceEventTypes.Update(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Delete a device event type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            DeviceEventType? foundObject = context.DeviceEventTypes.FirstOrDefault(e => e.Id == id);

            if (foundObject == null)
            {
                return NotFound();
            }

            context.DeviceEventTypes.Remove(foundObject);
            context.SaveChanges();

            return Ok();
        }
    }
}

