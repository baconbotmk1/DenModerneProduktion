using System.Diagnostics;
using Shared.DTOs.DeviceType;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/device_types")]
    public class DeviceTypesController : BaseCRUDController<DeviceType>
    {
        public DeviceTypesController(DataContext Context, IRepository<DeviceType> DIrepository) : base(Context, DIrepository)
        {
        }

        [HttpGet]
        public ActionResult<IEnumerable<DeviceType>> Get()
        {
            return HandleExceptions(() =>
            {
                var data = context.DeviceTypes
                    .AsNoTracking()
                    .ToList();

                return Ok(data);
            });
        }

        [HttpPost]
        public ActionResult<DeviceType> Post([FromBody] CreateDeviceType data)
        {
            return HandleExceptions(() =>
            {
                DeviceType item = data.Adapt<DeviceType>();

                repository.Insert(item);
                repository.Save();

                return Ok(item);
            });
        }


        [HttpGet("{id}")]
        public ActionResult<DeviceType> GetById(int id)
        {
            return HandleExceptions(() =>
            {
                DeviceType? deviceType = repository.GetById(id);

                if (deviceType == null)
                {
                    return NotFound();
                }

                return Ok(deviceType);
            });
        }


        [HttpPut("{id}")]
        public ActionResult<DeviceType> Put(int id, [FromBody] CreateDeviceType data)
        {
            try
            {
                DeviceType? item = repository.GetById(id);

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
                DeviceType? foundObject = repository.GetById(id);

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

