using Shared.DTOs.Cadastre;
using Shared.DTOs.DataLimit;

namespace SystemAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_context"></param>
    /// <param name="_configuration"></param>
    /// <param name="_provider"></param>
    [ApiController]
    [Route("api/cadastres")]
    public class CadastresController(DataContext _context, IConfiguration _configuration, IServiceProvider _provider) : BaseController(_context, _configuration, _provider)
    {
        /// <summary>
        /// Get all cadastres
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Cadastre> Get()
        {
            return context.Cadastres
                .AsQueryable()
                .Include(e => e.Buildings)
                .Include(e => e.LimitValues)
                .ToList();
        }

        /// <summary>
        /// Create a new cadastre
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Cadastre> Post([FromBody] CreateCadastre data)
        {
            Cadastre item = data.Adapt<Cadastre>();

            context.Cadastres.Add(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Get a cadastre by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Cadastre> GetCadastreById(int id)
        {
            Cadastre? Cadastre = context.Cadastres
                .AsQueryable()
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

        /// <summary>
        /// Update a cadastre
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<Cadastre> Put(int id, [FromBody] CreateCadastre data)
        {
            Cadastre? item = context.Cadastres.FirstOrDefault(e => e.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            data.Adapt(item);

            context.Cadastres.Update(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Delete a cadastre
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Cadastre? foundObject = context.Cadastres.FirstOrDefault(e => e.Id == id);

            if (foundObject == null)
            {
                return NotFound();
            }

            context.Cadastres.Remove(foundObject);
            context.SaveChanges();

            return Ok();
        }
    }
}

