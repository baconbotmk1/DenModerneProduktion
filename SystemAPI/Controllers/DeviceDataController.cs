using Shared.DTOs.Device;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/device_info")]
    public class DeviceDataController(DataContext _context, IConfiguration _configuration, IServiceProvider _provider) : BaseController(_context, _configuration, _provider)
    {
        /// <summary>
        /// Get all device info for a device
        /// </summary>
        /// <param name="device_id"></param>
        /// <param name="skip">Skip the first x entries</param>
        /// <param name="limit">Take only x entries</param>
        /// <param name="order_by">time_asc, time_desc, type_asc, type_desc</param>
        /// <returns></returns>
        [HttpGet("{device_id}/info")]
        public ActionResult<IEnumerable<DeviceInfo>> GetInfo( int device_id, [FromQuery] int skip = 0, int limit = 100, string order_by = "time_desc" )
        {
            var data = context.DeviceInfos
                .AsQueryable()
                .Where(e => device_id > 0 ? e.DeviceId == device_id : true)
                .OrderByDescending(e => e.Timestamp)
                .Skip(skip)
                .Take(limit);

            switch(order_by)
            {
                case "time_asc":
                    data = data.OrderBy(e => e.Timestamp);
                    break;
                case "time_desc":
                    data = data.OrderByDescending(e => e.Timestamp);
                    break;
                case "type_asc":
                    data = data.OrderBy(e => e.TypeId);
                    break;
                case "type_desc":
                    data = data.OrderByDescending(e => e.TypeId);
                    break;
            }

            var result = data
                .Include(e => e.Type)
                    .ThenInclude(e => e.Category)
                .ToList();

            return Ok(result);
        }

        /// <summary>
        /// Get all device data for a device
        /// </summary>
        /// <param name="device_id"></param>
        /// <param name="skip">Skip the first x entries</param>
        /// <param name="limit">Take only x entries</param>
        /// <param name="order_by">time_asc, time_desc, type_asc, type_desc</param>
        /// <returns></returns>
        [HttpGet("{device_id}/data")]
        public ActionResult<IEnumerable<DeviceData>> GetData(int device_id, [FromQuery] int skip = 0, int limit = 100, string order_by = "time_desc")
        {
            var data = context.DeviceDatas
                .AsQueryable()
                .Where(e => device_id > 0 ? e.DeviceId == device_id : true)
                .OrderByDescending(e => e.Timestamp)
                .Skip(skip)
                .Take(limit);

            switch (order_by)
            {
                case "time_asc":
                    data = data.OrderBy(e => e.Timestamp);
                    break;
                case "time_desc":
                    data = data.OrderByDescending(e => e.Timestamp);
                    break;
                case "type_asc":
                    data = data.OrderBy(e => e.TypeId);
                    break;
                case "type_desc":
                    data = data.OrderByDescending(e => e.TypeId);
                    break;
            }

            var result = data
                .Include(e => e.Type)
                    .ThenInclude(e => e.Category)
                .ToList();

            return Ok(result);
        }

        /// <summary>
        /// Get all device events for a device
        /// </summary>
        /// <param name="device_id"></param>
        /// <param name="skip">Skip the first x entries</param>
        /// <param name="limit">Take only x entries</param>
        /// <param name="order_by">time_asc, time_desc, type_asc, type_desc</param>
        /// <returns></returns>
        [HttpGet("{device_id}/events")]
        public ActionResult<IEnumerable<DeviceEvent>> GetEvents(int device_id, [FromQuery] int skip = 0, int limit = 100, string order_by = "time_desc")
        {
            var data = context.DeviceEvents
                .AsQueryable()
                .Where(e => device_id > 0 ? e.DeviceId == device_id : true)
                .OrderByDescending(e => e.Timestamp)
                .Skip(skip)
                .Take(limit);

            switch (order_by)
            {
                case "time_asc":
                    data = data.OrderBy(e => e.Timestamp);
                    break;
                case "time_desc":
                    data = data.OrderByDescending(e => e.Timestamp);
                    break;
                case "type_asc":
                    data = data.OrderBy(e => e.TypeId);
                    break;
                case "type_desc":
                    data = data.OrderByDescending(e => e.TypeId);
                    break;
            }

            var result = data
                .Include(e => e.Type)
                    .ThenInclude(e => e.Category)
                .ToList();

            return Ok(result);
        }
    }
}

