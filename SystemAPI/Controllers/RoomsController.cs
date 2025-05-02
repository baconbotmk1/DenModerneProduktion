using Shared.DTOs.Room;
using Shared.DTOs.DataLimit;

namespace SystemAPI.Controllers
{
    /// <summary>
    /// Controller for managing rooms
    /// </summary>
    /// <param name="_context"></param>
    /// <param name="_configuration"></param>
    /// <param name="_provider"></param>
    [ApiController]
    [Route("api/rooms")]
    public class RoomsController(DataContext _context, IConfiguration _configuration, IServiceProvider _provider) : BaseController(_context, _configuration, _provider)
    {
        /// <summary>
        /// Get all rooms
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Room> Get()
        {
            return context.Rooms
                .AsNoTracking()
                .Include(e => e.Section)
                .ToList();
        }
        [HttpGet("/data")]
        public ActionResult<IEnumerable<Room>> GetAllDeviceData()
        {
            List<Room> data = context.Rooms
                .AsNoTracking()
                .Include(x => x.Devices)
                    .ThenInclude(x => x.Type)
                .Include(x => x.Devices)
                    .ThenInclude(x => x.Data)
                        .ThenInclude(x => x.Type)
                .Include(x => x.Devices)
                    .ThenInclude(x => x.Infos)
                        .ThenInclude(x => x.Type)
                .Include(x => x.LimitValues)
                .ToList();

            return Ok(data);
        }


        /// <summary>
        /// Create a new room
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Room> Post([FromBody] CreateRoom data)
        {
            Room item = data.Adapt<Room>();

            context.Rooms.Add(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Get a room by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Room> GetRoomByID(int id)
        {
            Room? room = context.Rooms
                .AsQueryable()
                .Include(x=>x.LimitValues)
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        /// <summary>
        /// Delete a room by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Room? foundObject = context.Rooms.FirstOrDefault(e => e.Id == id);

            if (foundObject == null)
            {
                return NotFound();
            }

            context.Rooms.Remove(foundObject);
            context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Update a room by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<Room> Put(int id, [FromBody] CreateRoom data)
        {
            Room? item = context.Rooms.FirstOrDefault(e => e.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            data.Adapt(item);

            context.Rooms.Update(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Get all device data by room id
        /// </summary>
        /// <param name="RoomId"></param>
        /// <param name="deviceDataType"></param>
        /// <param name="periodType">This parameter tells what period is used. 1 = daily, 2 = weekly, 3 = monthly</param>
        /// <returns></returns>
        [HttpGet("{room_id}/data")]
        public ActionResult<IEnumerable<DeviceData>> GetDataByRoomId(int room_id, [FromQuery] int deviceDataType = 2, [FromQuery] int periodType = 2)
        {
            var deviceIds = context.Devices
                .AsQueryable()
                .Where(x => x.RoomId == room_id)
                .Select(x => x.Id)
                .ToList();
            var fromTimestamp = DateTime.Now.AddDays(periodType == 0 ? -1 : periodType == 1 ? -7 : -30);
            var data = context.DeviceDatas
                .AsQueryable()
                .Include(x => x.Type)
                .Where(x => x.TypeId == deviceDataType && deviceIds.Contains(x.DeviceId))
                .Where(x => x.Timestamp > fromTimestamp)
                .OrderByDescending(e => e.Timestamp);
            return Ok(data.ToList());
        }

        /// <summary>
        /// Update or create a room data limit
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("{id}/limit")]
        public ActionResult<Room> UpdateOrCreateRoomLimit(int id, [FromBody] UpdateDataLimit data)
        {
            DeviceDataLimitValue? dDLV = context.DeviceDataLimit
                .AsQueryable()
                .Where(x => x.TypeId == data.DataTypeId && x.RoomId == id)
                .FirstOrDefault();

            if (dDLV == null)
            {
                context.DeviceDataLimit.Add(new()
                {
                    MaximumLimit = data.MaximumLimit,
                    MinimumLimit = data.MinimumLimit,
                    RoomId = id,
                    TypeId = data.DataTypeId
                });
            }
            else
            {
                data.Adapt(dDLV);
                context.Attach(dDLV);
            }
            context.SaveChanges();

            return Ok(dDLV);
        }
    }
}

