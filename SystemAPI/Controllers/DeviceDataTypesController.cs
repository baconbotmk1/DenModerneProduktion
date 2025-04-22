using System.Diagnostics;
using Shared.DTOs.DeviceDataType;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/device_data_types")]
    public class DeviceDataTypesController : BaseController<DeviceDataType>
    {
        public DeviceDataTypesController(DataContext Context, IRepository<DeviceDataType> DIrepository) : base(Context, DIrepository)
        {
        }

        [HttpGet]
        public ActionResult<IEnumerable<DeviceDataType>> Get()
        {
            return HandleExceptions(() =>
            {
                var data = context.DeviceDataTypes
                    .AsNoTracking()
                    .Include(e => e.Category)
                    .ToList();

                return Ok(data);
            });
        }

        [HttpPost]
        public ActionResult<DeviceDataType> Post([FromBody] CreateDeviceDataType data)
        {
            return HandleExceptions(() =>
            {
                DeviceDataType item = data.Adapt<DeviceDataType>();

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
                DeviceDataType? deviceDataType = repository.GetById(id);

                if (deviceDataType == null)
                {
                    return NotFound();
                }

                return Ok(deviceDataType);
            });
        }


        [HttpPut("{id}")]
        public ActionResult<DeviceDataType> Put(int id, [FromBody] CreateDeviceDataType data)
        {
            try
            {
                DeviceDataType? item = repository.GetById(id);

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
                DeviceDataType? foundObject = repository.GetById(id);

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

