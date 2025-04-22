using System.Diagnostics;
using Shared.DTOs.DeviceSharedCategory;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/device_shared_categories")]
    public class DeviceSharedCategoriesController : BaseController<DeviceSharedCategory>
    {
        public DeviceSharedCategoriesController(DataContext Context, IRepository<DeviceSharedCategory> DIrepository) : base(Context, DIrepository)
        {
        }

        [HttpGet]
        public ActionResult<IEnumerable<DeviceSharedCategory>> Get()
        {
            return HandleExceptions(() =>
            {
                var data = context.DeviceSharedCategories
                    .AsNoTracking()
                    .Include(e => e.InfoTypes)
                    .Include(e => e.DataTypes)
                    .Include(e => e.EventTypes)
                    .ToList();

                return Ok(data);
            });
        }

        [HttpPost]
        public ActionResult<DeviceSharedCategory> Post([FromBody] CreateDeviceSharedCategory data)
        {
            return HandleExceptions(() =>
            {
                DeviceSharedCategory item = data.Adapt<DeviceSharedCategory>();

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
                DeviceSharedCategory? deviceSharedCategory = repository.GetById(id);

                if (deviceSharedCategory == null)
                {
                    return NotFound();
                }

                return Ok(deviceSharedCategory);
            });
        }


        [HttpPut("{id}")]
        public ActionResult<DeviceSharedCategory> Put(int id, [FromBody] CreateDeviceSharedCategory data)
        {
            try
            {
                DeviceSharedCategory? item = repository.GetById(id);

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
                DeviceSharedCategory? foundObject = repository.GetById(id);

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

