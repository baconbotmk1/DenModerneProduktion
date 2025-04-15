using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Models;
using Shared.DTOs;
using Shared.Services;
using Mapster;
using Swashbuckle.AspNetCore.Filters;
using SystemAPI.SwaggerExamples;
using Microsoft.EntityFrameworkCore;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/sections")]
    public class SectionsController : BaseController<Section>
    {
        public SectionsController(DataContext Context, IRepository<Section> DIrepository) : base(Context, DIrepository)
        {
        }

        [HttpGet]
        public IEnumerable<Section> Get()
        {
            return context.Sections
                .AsNoTracking()
                .Include(e => e.Rooms)
                .Include(e => e.Building)
                .ToList();
        }

        [HttpPost]
        [SwaggerRequestExample(typeof(CreateSection), typeof(CreateSectionExample))]
        public ActionResult<Section> Post([FromBody] CreateSection data)
        {
            Section item = data.Adapt<Section>();

            repository.Insert(item);
            repository.Save();

            return Ok(item);
        }


        [HttpGet("{id}")]
        public ActionResult<Section> GetSectionById(int id)
        {
            Section? section = repository.GetById(id);

            if (section == null)
            {
                return NotFound();
            }

            return Ok(section);
        }


        [HttpPut("{id}")]
        public ActionResult<Section> Put(int id, [FromBody] CreateSection data)
        {
            Section? item = repository.GetById(id);

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
            Section? foundObject = repository.GetById(id);

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

