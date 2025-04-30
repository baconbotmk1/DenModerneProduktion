using Shared.DTOs.DeviceInfoType;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/device_info_types")]
    public class DeviceInfoTypesController(DataContext _context, IConfiguration _configuration, IServiceProvider _provider) : BaseController(_context, _configuration, _provider)
    {
        /// <summary>
        /// Get all device info types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<DeviceInfoType>> Get()
        {
            var data = context.DeviceInfoTypes
                .AsNoTracking()
                .Include(e => e.Category)
                .ToList();

            return Ok(data);
        }

        /// <summary>
        /// Create a new device info type
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DeviceInfoType> Post([FromBody] CreateDeviceInfoType data)
        {
            DeviceInfoType item = data.Adapt<DeviceInfoType>();

            context.DeviceInfoTypes.Add(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Get a device info type by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<DeviceInfoType> GetById(int id)
        {
            DeviceInfoType? deviceDataType = context.DeviceInfoTypes.FirstOrDefault(e => e.Id == id);

            if (deviceDataType == null)
            {
                return NotFound();
            }

            return Ok(deviceDataType);
        }

        /// <summary>
        /// Update a device info type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<DeviceInfoType> Put(int id, [FromBody] CreateDeviceInfoType data)
        {
            DeviceInfoType? item = context.DeviceInfoTypes.FirstOrDefault(e => e.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            data.Adapt(item);

            context.DeviceInfoTypes.Update(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Delete a device info type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            DeviceInfoType? foundObject = context.DeviceInfoTypes.FirstOrDefault(e => e.Id == id);

            if (foundObject == null)
            {
                return NotFound();
            }

            context.DeviceInfoTypes.Remove(foundObject);
            context.SaveChanges();

            return Ok();
        }
    }
}

