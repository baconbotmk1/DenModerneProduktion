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
    [Route("api/cadastres")]
    public class CadastresController : BaseController<Cadastre>
    {
        public CadastresController(DataContext Context, IRepository<Cadastre> DIrepository) : base(Context, DIrepository)
        {
        }

        [HttpGet]
        public IEnumerable<Cadastre> Get()
        {
            return context.Cadastres
                .AsNoTracking()
                .Include(e => e.Buildings)
                .ToList();
        }

        //[SwaggerRequestExample(typeof(CreateCadastre), typeof(CreateCadastreExample))]
        [HttpPost]
        public ActionResult<Cadastre> Post([FromBody] CreateCadastre data)
        {
            Cadastre item = data.Adapt<Cadastre>();

            repository.Insert(item);
            repository.Save();

            return Ok(item);
        }


        [HttpGet("{id}")]
        public ActionResult<Cadastre> GetCadastreById(int id)
        {
            Cadastre? Cadastre = repository.GetById(id);

            if (Cadastre == null)
            {
                return NotFound();
            }

            return Ok(Cadastre);
        }


        [HttpPut("{id}")]
        public ActionResult<Cadastre> Put(int id, [FromBody] CreateCadastre data)
        {
            Cadastre? item = repository.GetById(id);

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
            Cadastre? foundObject = repository.GetById(id);

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

