using Shared.DTOs.AccessCard;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/access_cards")]
    public class AccessCardsController(DataContext _context, IConfiguration _configuration, IServiceProvider _provider) : BaseController(_context, _configuration, _provider)
    {
        /// <summary>
        /// Get all access cards
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<AccessCard> Get()
        {
            return context.AccessCards
                .AsQueryable()
                .ToList();
        }

        /// <summary>
        /// Create a new access card
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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

            context.AccessCards.Add(item);
            context.SaveChanges();

            return Ok(item);
        }


        /// <summary>
        /// Get an access card by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<AccessCard> GetUserById(int id)
        {
            AccessCard? accessCard = context.AccessCards.FirstOrDefault(e => e.Id == id);

            if (accessCard == null)
            {
                return NotFound();
            }

            return Ok(accessCard);
        }

        /// <summary>
        /// Update an access card
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<AccessCard> Put(int id, [FromBody] CreateAccessCard data)
        {
            AccessCard? item = context.AccessCards.FirstOrDefault(e => e.Id == id);

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

            context.AccessCards.Update(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Delete an access card
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            AccessCard? foundObject = context.AccessCards.FirstOrDefault(e => e.Id == id);

            if (foundObject == null)
            {
                return NotFound();
            }

            context.AccessCards.Remove(foundObject);
            context.SaveChanges();

            return Ok();
        }
    }
}

