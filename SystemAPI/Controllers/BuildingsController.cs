using Shared.DTOs.Building;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/buildings")]
    public class BuildingsController : BaseCRUDController<Building>
    {
        public BuildingsController(DataContext Context, IRepository<Building> DIrepository) : base(Context, DIrepository)
        {
        }

        [HttpGet]
        public IEnumerable<Building> Get()
        {
            return context.Buildings
                .AsNoTracking()
                .Include(e => e.Cadastre)
                .Include(e => e.Sections)
                    .ThenInclude(e => e.Rooms)
                .ToList();
        }

        //[SwaggerRequestExample(typeof(CreateBuilding), typeof(CreateBuildingExample))]
        [HttpPost]
        public ActionResult<Building> Post([FromBody] CreateBuilding data)
        {
            Building item = data.Adapt<Building>();

            if(context.Cadastres.Count(e => e.Id == item.CadastreId) == 0)
            {
                return NotFound("No cadastre with that ID exists");
            }

            repository.Insert(item);
            repository.Save();

            return Ok(item);
        }


        [HttpGet("{id}")]
        public ActionResult<Building> GetBuildingById(int id)
        {
            Building? Building = repository.GetById(id);

            if (Building == null)
            {
                return NotFound();
            }

            return Ok(Building);
        }


        [HttpPut("{id}")]
        public ActionResult<Building> Put(int id, [FromBody] CreateBuilding data)
        {
            Building? item = repository.GetById(id);

            if (item == null)
            {
                return NotFound();
            }

            data.Adapt(item);

            if (context.Cadastres.Count(e => e.Id == item.CadastreId) == 0)
            {
                return NotFound("No Cadastre with that ID exists");
            }

            repository.Update(item);
            repository.Save();

            return Ok(item);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Building? foundObject = repository.GetById(id);

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

