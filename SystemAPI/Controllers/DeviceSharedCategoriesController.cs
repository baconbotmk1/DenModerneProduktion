using Shared.DTOs.DeviceSharedCategory;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/device_shared_categories")]
    public class DeviceSharedCategoriesController(DataContext _context, IConfiguration _configuration, IServiceProvider _provider) : BaseController(_context, _configuration, _provider)
    {
        /// <summary>
        /// Get all device shared categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<DeviceSharedCategory>> Get()
        {
            var data = context.DeviceSharedCategories
                .AsNoTracking()
                .Include(e => e.InfoTypes)
                .Include(e => e.DataTypes)
                .Include(e => e.EventTypes)
                .ToList();

            return Ok(data);
        }

        /// <summary>
        /// Create a new device shared category
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DeviceSharedCategory> Post([FromBody] CreateDeviceSharedCategory data)
        {
            DeviceSharedCategory item = data.Adapt<DeviceSharedCategory>();

            context.DeviceSharedCategories.Add(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Get a device shared category by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<DeviceSharedCategory> GetById(int id)
        {
            DeviceSharedCategory? deviceSharedCategory = context.DeviceSharedCategories.FirstOrDefault(e => e.Id == id);

            if (deviceSharedCategory == null)
            {
                return NotFound();
            }

            return Ok(deviceSharedCategory);
        }

        /// <summary>
        /// Update a device shared category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<DeviceSharedCategory> Put(int id, [FromBody] CreateDeviceSharedCategory data)
        {
            DeviceSharedCategory? item = context.DeviceSharedCategories.FirstOrDefault(e => e.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            data.Adapt(item);

            context.DeviceSharedCategories.Update(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Delete a device shared category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            DeviceSharedCategory? foundObject = context.DeviceSharedCategories.FirstOrDefault(e => e.Id == id);

            if (foundObject == null)
            {
                return NotFound();
            }

            context.DeviceSharedCategories.Remove(foundObject);
            context.SaveChanges();

            return Ok();
        }
    }
}

