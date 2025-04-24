using Microsoft.Extensions.DependencyModel;
using Shared.DTOs.Room;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomsController : BaseCRUDController<Room>
    {
        public RoomsController(DataContext Context, IRepository<Room> DIrepository) : base(Context, DIrepository)
        {
        }

        [HttpGet]
        public IEnumerable<Room> Get()
        {
            return context.Rooms
                .AsNoTracking()
                .Include(e => e.Section)
                .ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RoomId"></param>
        /// <param name="deviceDataType"></param>
        /// <param name="periodType">This parameter tells what period is used. 1 = daily, 2 = weekly, 3 = monthly</param>
        /// <returns></returns>
        [HttpGet("{room_id}/data")]
        public ActionResult<IEnumerable<DeviceData>> GetDataByRoomId(int room_id, [FromQuery] int deviceDataType = 2, [FromQuery] int periodType = 2)
        {
            return HandleExceptions(() =>
            {
                var deviceIds = context.Devices
                    .AsQueryable()
                    .Where(x => x.RoomId == room_id)
                    .Select(x => x.Id)
                    .ToList();
                var fromTimestamp = DateTime.Now.AddDays(periodType == 0 ? -1 : periodType == 1 ? -7 : -30);
                var data = context.DeviceDatas
                    .AsQueryable()
                    .Include(x=>x.Type)
                    .Where(x => x.TypeId == deviceDataType && deviceIds.Contains(x.DeviceId))
                    .Where(x => x.Timestamp > fromTimestamp)
                    .OrderByDescending(e => e.Timestamp);
                return Ok(data.ToList());
            });
        }

        [HttpPost]
        public ActionResult<Room> Post([FromBody] CreateRoom data)
        {
            Room item = data.Adapt<Room>();

            repository.Insert(item);
            repository.Save();

            return Ok(item);
        }


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


        [HttpPut("{id}")]
        public ActionResult<Room> Put(int id, [FromBody] CreateRoom data)
        {
            Room? item = repository.GetById(id);

            if (item == null)
            {
                return NotFound();
            }

            data.Adapt(item);

            repository.Update(item);
            repository.Save();

            return Ok(item);
        }

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

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Room? foundObject = repository.GetById(id);

            if (foundObject == null)
            {
                return NotFound();
            }

            bool deleted = repository.Delete(id);
            repository.Save();

            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}

