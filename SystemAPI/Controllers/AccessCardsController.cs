using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Models;
using Shared.DTOs;
using Shared.Services;
using Mapster;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/access_cards")]
    public class AccessCardsController : BaseController<AccessCard>
    {
        public AccessCardsController(DataContext Context, IRepository<AccessCard> DIrepository) : base(Context, DIrepository)
        {
        }

        [HttpGet]
        public IEnumerable<AccessCard> Get()
        {
            return repository.Get();
        }

        [HttpPost]
        public ActionResult<AccessCard> Post([FromBody] CreateAccessCard data)
        {
            if(data.UserId != null)
            {
                if(context.Users.Count(e => e.Id == data.UserId) == 0)
                {
                    return NotFound("User ID doesn't exist");
                }
            }

            AccessCard item = data.Adapt<AccessCard>();

            repository.Insert(item);
            repository.Save();

            return Ok(item);
        }


        [HttpGet("{id}")]
        public ActionResult<AccessCard> GetUserById(int id)
        {
            AccessCard? accessCard = repository.GetById(id);

            if (accessCard == null)
            {
                return NotFound();
            }

            return Ok(accessCard);
        }


        [HttpPut("{id}")]
        public ActionResult<AccessCard> Put(int id, [FromBody] CreateAccessCard data)
        {
            AccessCard? item = repository.GetById(id);

            if (item == null)
            {
                return NotFound();
            }

            if (data.UserId != null)
            {
                if (context.Users.Count(e => e.Id == data.UserId) == 0)
                {
                    return NotFound("User ID doesn't exist");
                }
            }

            data.Adapt(item);

            repository.Update(item);
            repository.Save();

            return Ok(item);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            AccessCard? foundObject = repository.GetById(id);

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

