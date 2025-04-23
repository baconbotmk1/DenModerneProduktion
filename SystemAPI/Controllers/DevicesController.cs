using System.Diagnostics;
using Shared.DTOs.Device;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DevicesController : BaseCRUDController<Device>
    {
        public DevicesController(DataContext Context, IRepository<Device> DIrepository) : base(Context, DIrepository)
        {
        }

        [HttpGet]
        public ActionResult<IEnumerable<Device>> Get()
        {
            return HandleExceptions(() =>
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
            });
        }

        [HttpPost]
        public ActionResult<Device> Post([FromBody] CreateDevice data)
        {
            return HandleExceptions(() =>
            {
                Device item = data.Adapt<Device>();

                repository.Insert(item);
                repository.Save();

                return Ok(item);
            });
        }


        [HttpGet("{id}")]
        public ActionResult<Device> GetById(int id)
        {
            return HandleExceptions(() =>
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
            });
        }


        [HttpPut("{id}")]
        public ActionResult<Device> Put(int id, [FromBody] CreateDevice data)
        {
            try
            {
                Device? item = repository.GetById(id);

                if (item == null)
                {
                    return NotFound();
                }

                data.Adapt(item);

                repository.Update(item);
                repository.Save();

                return Ok(item);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.GetType().Name);
                Debug.WriteLine(ex.GetType().Namespace);
                Debug.WriteLine(ex.Message);
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return HandleExceptions(() =>
            {
                Device? foundObject = repository.GetById(id);

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
            });
        }


        [HttpPost("{id}/info")]
        public ActionResult<DeviceInfo> PostInfo(int id, [FromBody] Shared.DTOs.DeviceInfo.CreateDeviceInfo data)
        {
            return HandleExceptions(() =>
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
            });
        }

        [HttpPost("{id}/data")]
        public ActionResult<DeviceData> PostData(int id, [FromBody] Shared.DTOs.DeviceData.CreateDeviceData data)
        {
            return HandleExceptions(() =>
            {
                DeviceData item = data.Adapt<DeviceData>();

                context.DeviceDatas.Add(item);
                context.SaveChanges();

                return Ok(item);
            });
        }

        [HttpPost("{id}/event")]
        public ActionResult<DeviceEvent> PostEvent(int id, [FromBody] Shared.DTOs.DeviceEvent.CreateDeviceEvent data)
        {
            return HandleExceptions(() =>
            {
                DeviceEvent item = data.Adapt<DeviceEvent>();

                context.DeviceEvents.Add(item);
                context.SaveChanges();

                return Ok(item);
            });
        }
    }
}

