using System.Diagnostics;
using Shared.DTOs.User;
using Swashbuckle.AspNetCore.Filters;
using SystemAPI.SwaggerExamples;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : BaseController<User>
    {
        public UsersController(DataContext Context, IRepository<User> DIrepository) : base(Context, DIrepository)
        {
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            return HandleExceptions(() =>
            {
                var data = context.Users
                .AsNoTracking()
                .Include(e => e.UserSecurityGroups)
                    .ThenInclude(e => e.SecurityGroup)
                        .ThenInclude(e => e.SecurityGroupPermissions)
                            .ThenInclude(e => e.Permission)
                .Include(e => e.AccessCards)
                .ToList();

                return Ok(data);
            });
        }

        [HttpPost]
        [SwaggerRequestExample(typeof(CreateUser), typeof(CreateUserExample))]
        public ActionResult<User> Post([FromBody] CreateUser data)
        {
            return HandleExceptions(() =>
            {
                User item = data.Adapt<User>();

                repository.Insert(item);
                repository.Save();

                return Ok(item);
            });
        }


        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            return HandleExceptions(() =>
            {
                User? accessCard = repository.GetById(id);

                if (accessCard == null)
                {
                    return NotFound();
                }

                return Ok(accessCard);
            });
        }


        [HttpPut("{id}")]
        public ActionResult<User> Put(int id, [FromBody] CreateUser data)
        {
            try
            {
                User? item = repository.GetById(id);

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
                User? foundObject = repository.GetById(id);

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

