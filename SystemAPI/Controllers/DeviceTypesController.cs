using Shared.DTOs.DeviceType;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/device_types")]
    public class DeviceTypesController(DataContext _context, IConfiguration _configuration, IServiceProvider _provider) : BaseController(_context, _configuration, _provider)
    {
        /// <summary>
        /// Get all device types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<DeviceType>> Get()
        {
            var data = context.DeviceTypes
                .AsNoTracking()
                .ToList();

            return Ok(data);
        }

        /// <summary>
        /// Create a new device type
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DeviceType> Post([FromBody] CreateDeviceType data)
        {
            DeviceType item = data.Adapt<DeviceType>();

            context.DeviceTypes.Add(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Get a device type by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<DeviceType> GetById(int id)
        {
            DeviceType? deviceType = context.DeviceTypes.FirstOrDefault(e => e.Id == id);

            if (deviceType == null)
            {
                return NotFound();
            }

            return Ok(deviceType);
        }

        /// <summary>
        /// Update a device type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<DeviceType> Put(int id, [FromBody] CreateDeviceType data)
        {
            DeviceType? item = context.DeviceTypes.FirstOrDefault(e => e.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            data.Adapt(item);

            context.DeviceTypes.Update(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Delete a device type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            DeviceType? foundObject = context.DeviceTypes.FirstOrDefault(e => e.Id == id);

            if (foundObject == null)
            {
                return NotFound();
            }

            context.DeviceTypes.Remove(foundObject);
            context.SaveChanges();


            return Ok();
        }
    }
}

