using Shared.DTOs.Section;

namespace SystemAPI.Controllers
{
    /// <summary>
    /// Controller for managing sections
    /// </summary>
    /// <param name="_context"></param>
    /// <param name="_configuration"></param>
    /// <param name="_provider"></param>
    [ApiController]
    [Route("api/sections")]
    public class SectionsController(DataContext _context, IConfiguration _configuration, IServiceProvider _provider) : BaseController(_context, _configuration, _provider)
    {
        /// <summary>
        /// Get all sections
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Section> Get()
        {
            return context.Sections
                .AsNoTracking()
                .Include(e => e.Rooms)
                .Include(e => e.Building)
                .ToList();
        }

        /// <summary>
        /// Create a new section
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Section> Post([FromBody] CreateSection data)
        {
            Section item = data.Adapt<Section>();

            context.Sections.Add(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Get a section by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Section> GetSectionById(int id)
        {
            Section? section = context.Sections.FirstOrDefault(e => e.Id == id);

            if (section == null)
            {
                return NotFound();
            }

            return Ok(section);
        }

        /// <summary>
        /// Update a section
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<Section> Put(int id, [FromBody] CreateSection data)
        {
            Section? item = context.Sections.FirstOrDefault(e => e.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            data.Adapt(item);

            context.Sections.Update(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Delete a section
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Section? foundObject = context.Sections.FirstOrDefault(e => e.Id == id);

            if (foundObject == null)
            {
                return NotFound();
            }

            context.Sections.Remove(foundObject);
            context.SaveChanges();

            return Ok();
        }
    }
}

