using Shared.DTOs.Permission;

namespace SystemAPI.Controllers
{
    /// <summary>
    /// Controller for managing permissions
    /// </summary>
    /// <param name="_context"></param>
    /// <param name="_configuration"></param>
    /// <param name="_provider"></param>
    [ApiController]
    [Route("api/permissions")]
    public class PermissionsController(DataContext _context, IConfiguration _configuration, IServiceProvider _provider) : BaseController(_context, _configuration, _provider)
    {
        /// <summary>
        /// Get all permissions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Permission> Get()
        {
            return context.Permissions
                .AsQueryable()
                .ToList();
        }

        /// <summary>
        /// Get a permission by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Permission> GetPermissionByID(int id)
        {
            Permission? item = context.Permissions.FirstOrDefault(e => e.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        /// <summary>
        /// Create a new permission
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Permission> Post([FromBody] CreatePermissionDTO data)
        {
            Permission item = data.Adapt<Permission>();

            context.Permissions.Add(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Update a permission
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        public ActionResult<Permission> Put(int id, [FromBody] CreatePermissionDTO body)
        {
            Permission? item = context.Permissions.FirstOrDefault(e => e.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            body.Adapt(item);

            context.Permissions.Update(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Delete a permission
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual ActionResult Delete(int id)
        {
            Permission? foundObject = context.Permissions.FirstOrDefault(e => e.Id == id);

            if (foundObject == null)
            {
                return NotFound();
            }

            context.Permissions.Remove(foundObject);
            context.SaveChanges();

            return Ok();
        }
    }
}

