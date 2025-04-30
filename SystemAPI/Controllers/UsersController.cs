using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
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
                    .Include(e => e.UserRooms)
                        .ThenInclude(e => e.Room)
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
                var data = context.Users
                    .AsQueryable()
                    .Include(e => e.UserSecurityGroups)
                        .ThenInclude(e => e.SecurityGroup)
                            .ThenInclude(e => e.SecurityGroupPermissions)
                                .ThenInclude(e => e.Permission)
                    .Include(e => e.AccessCards)
                    .Include(e => e.UserRooms)
                        .ThenInclude(e => e.Room)
                    .First(e => e.Id == id);

                if (data == null)
                {
                    return NotFound();
                }

                return Ok(data);
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
            catch (Exception ex)
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

            return Ok(new object { });
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

            return Ok(new object { });
        }
        [HttpGet("{id}/rooms")]
        public ActionResult<IEnumerable<Room>> GetUserRooms(int id)
        {
            List<Room> data = new();
            var viewAllPermission = context.Permissions.FirstOrDefault(x => x.Slug == "rooms_view_all");

            if (viewAllPermission != null)
            {
                var hasViewAllPermission = context.UserSecurityGroups
                    .Include(x => x.SecurityGroup)
                        .ThenInclude(x => x.SecurityGroupPermissions)
                            .ThenInclude(x => x.Permission)
                    .Where(x => x.UserId == id && x.SecurityGroup.SecurityGroupPermissions.Where(y => y.Permission.Slug == "rooms_view_all").Count() > 0)
                    .Count() > 0;
                if (hasViewAllPermission)
                {
                    data = context.Rooms.ToList();
                    return Ok(data);
                }
            }

            var accessibleRoomsData = context.UserRooms
                .AsQueryable()
                .Include(x => x.User)
                .Include(x => x.Room)
                .Where(x => x.UserId == id)
                .ToList();

            data = context.Rooms.Where(x => accessibleRoomsData.Select(y => y.RoomId).Contains(x.Id)).ToList();

            return Ok(data);
        }

        [HttpGet("{id}/rooms/data")]
        public ActionResult<IEnumerable<Room>> GetRoomsWithDataByUserId(int id)
        {
            List<Room> data = new();
            bool hasViewAllPermission = false;
            var viewAllPermission = context.Permissions.FirstOrDefault(x => x.Slug == "rooms_view_all");

            if (viewAllPermission != null)
            {
                hasViewAllPermission = context.UserSecurityGroups
                    .Include(x => x.SecurityGroup)
                        .ThenInclude(x => x.SecurityGroupPermissions)
                            .ThenInclude(x => x.Permission)
                    .Where(x => x.UserId == id && x.SecurityGroup.SecurityGroupPermissions.Where(y => y.Permission.Slug == "rooms_view_all").Count() > 0)
                    .Count() > 0;
            }

            var accessibleRoomsData = context.UserRooms
                .AsQueryable()
                .Include(x => x.User)
                .Include(x => x.Room)
                .Where(x => x.UserId == id)
                .Select(x=>x.RoomId)
                .ToList();

            data = context.Rooms
                .AsNoTracking()
                .Include(x => x.Devices)
                    .ThenInclude(x=>x.Type)
                .Include(x=>x.Devices)
                    .ThenInclude(x => x.Data)
                        .ThenInclude(x => x.Type)
                .Include(x => x.Devices)
                    .ThenInclude(x => x.Infos)
                        .ThenInclude(x => x.Type)
                .Include(x => x.LimitValues)
                .Where(x => hasViewAllPermission ? true : accessibleRoomsData.Contains(x.Id))
                .ToList();

            return Ok(data);
        }

        [HttpPost("{id}/room/{room_id}")]
        public ActionResult AddRoom(int id, int room_id)
        {
            User? user = context.Users.Find(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            Room? room = context.Rooms.Find(room_id);
            if (room == null)
            {
                return NotFound("Room not found");
            }

            if (context.UserRooms.Count(e => e.UserId == id && e.RoomId == room_id) > 0)
            {
                return NotFound("User already have access to that room");
            }

            context.UserRooms.Add(new UserRoom()
            {
                UserId = id,
                RoomId = room_id
            });

            context.SaveChanges();

            return Ok(new object { });
        }

        [HttpDelete("{id}/room/{room_id}")]
        public ActionResult RemoveRoom(int id, int room_id)
        {
            User? user = context.Users.Find(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            Room? room = context.Rooms.Find(room_id);
            if (room == null)
            {
                return NotFound("Room not found");
            }

            UserRoom? link = context.UserRooms.FirstOrDefault(e => e.UserId == id && e.RoomId == room_id);
            if (link == null)
            {
                return NotFound("User dont have access to that room");
            }

            if (context.Entry(link).State == EntityState.Detached)
            {
                context.UserRooms.Attach(link);
            }

            context.UserRooms.Remove(link);
            context.SaveChanges();

            return Ok(new object { });
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

