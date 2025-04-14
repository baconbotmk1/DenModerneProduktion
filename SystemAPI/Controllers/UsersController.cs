using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Models;
using Shared.DTOs.User;
using Shared.Services;
using Mapster;

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
        public IEnumerable<User> Get()
        {
            return repository.Get();
        }

        [HttpPost]
        public ActionResult<User> Post([FromBody] CreateUser data)
        {
            User item = data.Adapt<User>();

            repository.Insert(item);
            repository.Save();

            return Ok(item);
        }


        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            User? accessCard = repository.GetById(id);

            if (accessCard == null)
            {
                return NotFound();
            }

            return Ok(accessCard);
        }


        [HttpPut("{id}")]
        public ActionResult<User> Put(int id, [FromBody] CreateUser data)
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

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
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
        }
    }
}

