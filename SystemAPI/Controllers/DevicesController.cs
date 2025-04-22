using System.Diagnostics;
using Shared.DTOs.Device;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DevicesController : BaseController<Device>
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
                    .AsNoTracking()
                    .Include(e => e.Type)
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
                Device? device = repository.GetById(id);

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
    }
}

