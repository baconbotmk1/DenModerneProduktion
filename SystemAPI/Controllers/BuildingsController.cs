using Shared.DTOs.Building;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/buildings")]
    public class BuildingsController(DataContext _context, IConfiguration _configuration, IServiceProvider _provider) : BaseController(_context, _configuration, _provider)
    {
        /// <summary>
        /// Get all buildings
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Create a new building
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Building> Post([FromBody] CreateBuilding data)
        {
            Building item = data.Adapt<Building>();

            if(context.Cadastres.Count(e => e.Id == item.CadastreId) == 0)
            {
                return NotFound("No cadastre with that ID exists");
            }

            context.Buildings.Add(item);
            context.SaveChanges();

            return Ok(item);
        }


        /// <summary>
        /// Get a building by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Building> GetBuildingById(int id)
        {
            Building? Building = context.Buildings.FirstOrDefault(e => e.Id == id);

            if (Building == null)
            {
                return NotFound();
            }

            return Ok(Building);
        }


        /// <summary>
        /// Update a building
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<Building> Put(int id, [FromBody] CreateBuilding data)
        {
            Building? item = context.Buildings.FirstOrDefault(e => e.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            data.Adapt(item);

            if (context.Cadastres.Count(e => e.Id == item.CadastreId) == 0)
            {
                return NotFound("No Cadastre with that ID exists");
            }

            context.Buildings.Update(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Delete a building
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Building? foundObject = context.Buildings.FirstOrDefault(e => e.Id == id);

            if (foundObject == null)
            {
                return NotFound();
            }

            context.Buildings.Remove(foundObject);
            context.SaveChanges();

            return Ok();
        }
    }
}

