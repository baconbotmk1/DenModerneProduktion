using Shared.DTOs.Cadastre;
using Shared.DTOs.DataLimit;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/cadastres")]
    public class CadastresController : BaseCRUDController<Cadastre>
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
                .Include(e => e.LimitValues)
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
            Cadastre? Cadastre = context.Cadastres
                .AsNoTracking()
                .Include(e => e.Buildings)
                .Include(e => e.LimitValues)
                .FirstOrDefault();

            if (Cadastre == null)
            {
                return NotFound();
            }

            return Ok(Cadastre);
        }
        [HttpPut("{id}/limit")]
        public ActionResult<Cadastre> UpdateOrCreateCadastreLimit(int id, [FromBody] UpdateDataLimit data)
        {
            DeviceDataLimitValue? dDLV = context.DeviceDataLimit
                .AsQueryable()
                .Where(x => x.TypeId == data.DataTypeId && x.CadastreId == id)
                .FirstOrDefault();

            if (dDLV == null)
            {
                context.DeviceDataLimit.Add(new()
                {
                    MaximumLimit = data.MaximumLimit,
                    MinimumLimit = data.MinimumLimit,
                    CadastreId = id,
                    TypeId = data.DataTypeId
                });
            }
            else
            {
                data.Adapt(dDLV);
                context.Attach(dDLV);
            }
            context.SaveChanges();

            return Ok(dDLV);
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

