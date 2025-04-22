using System.Diagnostics;
using Shared.DTOs.DeviceEventType;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/device_event_types")]
    public class DeviceEventTypesController : BaseController<DeviceEventType>
    {
        public DeviceEventTypesController(DataContext Context, IRepository<DeviceEventType> DIrepository) : base(Context, DIrepository)
        {
        }

        [HttpGet]
        public ActionResult<IEnumerable<DeviceEventType>> Get()
        {
            return HandleExceptions(() =>
            {
                var data = context.DeviceEventTypes
                    .AsNoTracking()
                    .Include(e => e.Category)
                    .ToList();

                return Ok(data);
            });
        }

        [HttpPost]
        public ActionResult<DeviceEventType> Post([FromBody] CreateDeviceEventType data)
        {
            return HandleExceptions(() =>
            {
                DeviceEventType item = data.Adapt<DeviceEventType>();

                repository.Insert(item);
                repository.Save();

                return Ok(item);
            });
        }


        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            return HandleExceptions(() =>
            {
                DeviceEventType? deviceDataType = repository.GetById(id);

                if (deviceDataType == null)
                {
                    return NotFound();
                }

                return Ok(deviceDataType);
            });
        }


        [HttpPut("{id}")]
        public ActionResult<DeviceEventType> Put(int id, [FromBody] CreateDeviceEventType data)
        {
            try
            {
                DeviceEventType? item = repository.GetById(id);

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
                DeviceEventType? foundObject = repository.GetById(id);

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
    }
}

