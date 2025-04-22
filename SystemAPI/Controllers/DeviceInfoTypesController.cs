using System.Diagnostics;
using Shared.DTOs.DeviceInfoType;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/device_info_types")]
    public class DeviceInfoTypesController : BaseController<DeviceInfoType>
    {
        public DeviceInfoTypesController(DataContext Context, IRepository<DeviceInfoType> DIrepository) : base(Context, DIrepository)
        {
        }

        [HttpGet]
        public ActionResult<IEnumerable<DeviceInfoType>> Get()
        {
            return HandleExceptions(() =>
            {
                var data = context.DeviceInfoTypes
                    .AsNoTracking()
                    .Include(e => e.Category)
                    .ToList();

                return Ok(data);
            });
        }

        [HttpPost]
        public ActionResult<DeviceInfoType> Post([FromBody] CreateDeviceInfoType data)
        {
            return HandleExceptions(() =>
            {
                DeviceInfoType item = data.Adapt<DeviceInfoType>();

                repository.Insert(item);
                repository.Save();

                return Ok(item);
            });
        }


        [HttpGet("{id}")]
        public ActionResult<DeviceInfoType> GetById(int id)
        {
            return HandleExceptions(() =>
            {
                DeviceInfoType? deviceDataType = repository.GetById(id);

                if (deviceDataType == null)
                {
                    return NotFound();
                }

                return Ok(deviceDataType);
            });
        }


        [HttpPut("{id}")]
        public ActionResult<DeviceInfoType> Put(int id, [FromBody] CreateDeviceInfoType data)
        {
            try
            {
                DeviceInfoType? item = repository.GetById(id);

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
                DeviceInfoType? foundObject = repository.GetById(id);

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

