using System.Diagnostics;
using Shared.DTOs.User;
using Swashbuckle.AspNetCore.Filters;
using SystemAPI.SwaggerExamples;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : BaseCRUDController<User>
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
                    .AsQueryable()
                    //.AsNoTracking()
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


        [HttpPost("{id}/security_group/{security_group_id}")]
        public ActionResult AddSecurityGroup(int id, int security_group_id)
        {
            User? user = context.Users.Find(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            SecurityGroup? security_group = context.SecurityGroups.Find(security_group_id);
            if (security_group == null)
            {
                return NotFound("Security Group not found");
            }

            if (context.UserSecurityGroups.Count(e => e.UserId == id && e.SecurityGroupId == security_group_id) > 0)
            {
                return NotFound("User is already part of that Security Group");
            }

            context.UserSecurityGroups.Add(new UserSecurityGroup()
            {
                UserId = id,
                SecurityGroupId = security_group_id
            });

            context.SaveChanges();

            return Ok();
        }


        [HttpDelete("{id}/security_group/{security_group_id}")]
        public ActionResult RemoveSecurityGroup(int id, int security_group_id)
        {
            User? user = context.Users.Find(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            SecurityGroup? security_group = context.SecurityGroups.Find(security_group_id);
            if (security_group == null)
            {
                return NotFound("Security Group not found");
            }

            if (context.UserSecurityGroups.Count(e => e.UserId == id && e.SecurityGroupId == security_group_id) > 0)
            {
                return NotFound("User is already part of that Security Group");
            }

            UserSecurityGroup? link = context.UserSecurityGroups.FirstOrDefault(e => e.UserId == id && e.SecurityGroupId == security_group_id);
            if (link == null)
            {
                return NotFound("User is not part of that Security Group");
            }

            if (context.Entry(link).State == EntityState.Detached)
            {
                context.UserSecurityGroups.Attach(link);
            }

            context.UserSecurityGroups.Remove(link);
            context.SaveChanges();

            return Ok();
        }


        [HttpPost("{id}/access_card/{access_card_id}")]
        public ActionResult AddAccessCard(int id, int access_card_id)
        {
            User? user = context.Users.Find(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            AccessCard? accessCard = context.AccessCards.Find(access_card_id);
            if (accessCard == null)
            {
                return NotFound("Access Card not found");
            }

            if (accessCard.UserId != null)
            {
                return NotFound("Access Card is already bound to a User");
            }

            accessCard.UserId = id;

            context.SaveChanges();

            return Ok();
        }


        [HttpDelete("{id}/access_card/{access_card_id}")]
        public ActionResult RemoveAccessCard(int id, int access_card_id)
        {
            User? user = context.Users.Find(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            AccessCard? accessCard = context.AccessCards.Find(access_card_id);
            if (accessCard == null)
            {
                return NotFound("Access Card not found");
            }

            if (accessCard.UserId != id)
            {
                return NotFound("Access Card is not bound to that User");
            }

            accessCard.UserId = null;

            context.SaveChanges();

            return Ok();
        }
    }
}

