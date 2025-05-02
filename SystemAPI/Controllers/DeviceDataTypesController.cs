using Shared.DTOs.DeviceDataType;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/device_data_types")]
    public class DeviceDataTypesController(DataContext _context, IConfiguration _configuration, IServiceProvider _provider) : BaseController(_context, _configuration, _provider)
    {
        /// <summary>
        /// Get all device data types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<DeviceDataType>> Get()
        {
            var data = context.DeviceDataTypes
                .AsNoTracking()
                .Include(e => e.Category)
                .ToList();

            return Ok(data);
        }

        /// <summary>
        /// Create a new device data type
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DeviceDataType> Post([FromBody] CreateDeviceDataType data)
        {
            DeviceDataType item = data.Adapt<DeviceDataType>();

            context.DeviceDataTypes.Add(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Get a device data type by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<DeviceDataType> GetById(int id)
        {
            DeviceDataType? deviceDataType = context.DeviceDataTypes.FirstOrDefault(e => e.Id == id);

            if (deviceDataType == null)
            {
                return NotFound();
            }

            return Ok(deviceDataType);
        }

        /// <summary>
        /// Update a device data type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<DeviceDataType> Put(int id, [FromBody] CreateDeviceDataType data)
        {
            DeviceDataType? item = context.DeviceDataTypes.FirstOrDefault(e => e.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            data.Adapt(item);

            context.DeviceDataTypes.Update(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Delete a device data type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            DeviceDataType? foundObject = context.DeviceDataTypes.FirstOrDefault(e => e.Id == id);

            if (foundObject == null)
            {
                return NotFound();
            }

            context.DeviceDataTypes.Remove(foundObject);
            context.SaveChanges();

            return Ok();
        }
    }
}

