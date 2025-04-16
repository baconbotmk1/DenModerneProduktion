using Shared.DTOs.Room;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomsController : BaseController<Room>
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

        [HttpPost]
        public ActionResult<Room> Post([FromBody] CreateRoom data)
        {
            Room item = data.Adapt<Room>();

            repository.Insert(item);
            repository.Save();

            return Ok(item);
        }


        [HttpGet("{id}")]
        public ActionResult<Room> GetSectionById(int id)
        {
            Room? section = repository.GetById(id);

            if (section == null)
            {
                return NotFound();
            }

            return Ok(section);
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

