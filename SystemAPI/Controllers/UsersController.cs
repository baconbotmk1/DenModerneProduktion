using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.DTOs.User;
using Swashbuckle.AspNetCore.Filters;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController(DataContext _context, IConfiguration _configuration, IServiceProvider _provider) : BaseController(_context, _configuration, _provider)
    {
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
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
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<User> Post([FromBody] CreateUser data)
        {
            User item = data.Adapt<User>();

            context.Users.Add(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Get a user by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
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
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<User> Put(int id, [FromBody] CreateUser data)
        {
            User? item = context.Users.FirstOrDefault(e => e.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            data.Adapt(item);

            context.Users.Update(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            User? foundObject = context.Users.FirstOrDefault(e => e.Id == id);

            if (foundObject == null)
            {
                return NotFound();
            }

            context.Users.Remove(foundObject);
            context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Add a security group to a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="security_group_id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Remove a security group from a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="security_group_id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get all rooms that a user has access to
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get all rooms that a user has access to, with data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add access to a room for a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="room_id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Remove access to a room for a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="room_id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Assign an access card to a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="access_card_id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Remove an access card from a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="access_card_id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Import users from an external source
        /// </summary>
        /// <param name="source_type"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        [HttpPost("import/{source_type}")]
        public ActionResult ImportUsers(string source_type, [FromBody] List<ImportedUser> users, [FromQuery] bool overrideExistingEmails = false)
        {
            List<User> currentUsers = context.Users
                .AsQueryable()
                .ToList();

            List<int> CheckedIDs = new List<int>();

            foreach (ImportedUser importedUser in users)
            {
                User? previouslyImported = currentUsers.FirstOrDefault(e => e.ReferenceType != null && e.ReferenceId != null && e.ReferenceType == source_type && e.ReferenceId == importedUser.Uuid);
                if (previouslyImported != null)
                {
                    Debug.WriteLine($"User {importedUser.Uuid} already imported previously");

                    CheckedIDs.Add(previouslyImported.Id);

                    previouslyImported.IsActive = true;
                    context.Update(previouslyImported);
                    context.SaveChanges();

                    continue;
                }

                User? sameEmail = currentUsers.FirstOrDefault(e => ( e.ReferenceType == null || e.ReferenceType != source_type || e.ReferenceId == null) && e.Username.ToLower() == importedUser.Email.ToLower());
                if (sameEmail != null)
                {
                    Debug.WriteLine($"A user with the email {importedUser.Email} already exists");

                    if(overrideExistingEmails)
                    {
                        Debug.WriteLine($"Overriding existing user");
                        CheckedIDs.Add(sameEmail.Id);

                        sameEmail.Name = importedUser.Name;
                        sameEmail.ReferenceType = source_type;
                        sameEmail.ReferenceId = importedUser.Uuid;
                        sameEmail.IsActive = true;

                        context.Update(sameEmail);
                        context.SaveChanges();
                    }

                    continue;
                }

                User newUser = new User()
                {
                    Name = importedUser.Name,
                    Username = importedUser.Email,
                    ReferenceType = source_type,
                    ReferenceId = importedUser.Uuid,
                    IsActive = true,
                };
                context.Users.Add(newUser);
                context.SaveChanges();
            }

            foreach(User remainingUser in currentUsers.Where(e => e.ReferenceType == source_type))
            {
                Debug.WriteLine($"Deactivating user {remainingUser.Username}, because he wasn't on the import list, but is part of the same reference type");

                remainingUser.IsActive = false;
                context.SaveChanges();
            }

            return Ok("Users imported & updated");
        }
    }
}

