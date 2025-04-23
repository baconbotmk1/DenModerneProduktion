using System.Diagnostics;
using Shared.DTOs.Device;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/device_info")]
    public class DeviceDataController : BaseCRUDController<Device>
    {
        public DeviceDataController(DataContext Context, IRepository<Device> DIrepository) : base(Context, DIrepository)
        {
        }

        [HttpGet("{device_id}/info")]
        public ActionResult<IEnumerable<DeviceInfo>> GetInfo( int device_id, [FromQuery] int skip = 0, int limit = 100, string order_by = "time_desc" )
        {
            return HandleExceptions(() =>
            {
                var data = context.DeviceInfos
                    .AsQueryable()
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
            });
        }

        [HttpGet("{device_id}/data")]
        public ActionResult<IEnumerable<DeviceData>> GetData(int device_id, [FromQuery] int skip = 0, int limit = 100, string order_by = "time_desc")
        {
            return HandleExceptions(() =>
            {
                var data = context.DeviceDatas
                    .AsQueryable()
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
            });
        }


        [HttpGet("{device_id}/events")]
        public ActionResult<IEnumerable<DeviceEvent>> GetEvents(int device_id, [FromQuery] int skip = 0, int limit = 100, string order_by = "time_desc")
        {
            return HandleExceptions(() =>
            {
                var data = context.DeviceEvents
                    .AsQueryable()
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
            });
        }
    }
}

