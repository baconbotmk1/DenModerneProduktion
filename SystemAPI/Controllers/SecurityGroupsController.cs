using System.Diagnostics;
using Shared.DTOs.SecurityGroup;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/security_groups")]
    public class SecurityGroupsController(DataContext _context, IConfiguration _configuration, IServiceProvider _provider) : BaseController(_context, _configuration, _provider)
    {
        /// <summary>
        /// Get all security groups
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<SecurityGroup>> Get()
        {
            var data = context.SecurityGroups
                .AsQueryable()
                .Include(e => e.SecurityGroupPermissions)
                    .ThenInclude(e => e.Permission)
                .Include(e => e.UserSecurityGroups)
                    .ThenInclude(e => e.User)
                .Include(e => e.SecurityGroupRooms)
                    .ThenInclude(e => e.Room)
                .ToList();

            return Ok(data);
        }

        /// <summary>
        /// Get a security group by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<SecurityGroup> GetUserById(int id)
        {
            SecurityGroup? securityGroup = context.SecurityGroups
                .AsQueryable()
                    .Include(e => e.SecurityGroupPermissions)
                        .ThenInclude(e => e.Permission)
                    .Include(e => e.UserSecurityGroups)
                        .ThenInclude(e => e.User)
                    .Include(e => e.SecurityGroupRooms)
                        .ThenInclude(e => e.TimeLimits)
                            .ThenInclude(e => e.Weeks)
                                .ThenInclude(e => e.Days)
                                    .ThenInclude(e => e.Times)
                    .Include(e => e.SecurityGroupRooms)
                        .ThenInclude(e => e.Room)
                    .Include(e => e.TimeLimits)
                        .ThenInclude(e => e.Weeks)
                            .ThenInclude(e => e.Days)
                                .ThenInclude(e => e.Times)
                .FirstOrDefault(e => e.Id == id);

            if(securityGroup == null)
            {
                return NotFound();
            }

            return Ok(securityGroup);
        }

        /// <summary>
        /// Create a new security group
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        public ActionResult<SecurityGroup> Post([FromBody]CreateSecurityGroupDTO data)
        {
            SecurityGroup group = data.Adapt<SecurityGroup>();

            context.SecurityGroups.Add(group);
            context.SaveChanges();

            return Ok(group);
        }

        /// <summary>
        /// Update a security group
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<SecurityGroup> Put(int id, [FromBody]CreateSecurityGroupDTO body)
        {
            SecurityGroup? item = context.SecurityGroups.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            body.Adapt(item);

            context.SecurityGroups.Attach(item);
            context.SaveChanges();

            return Ok(item);
        }

        /// <summary>
        /// Delete a security group
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            SecurityGroup? securityGroup = context.SecurityGroups.Find(id);

            if (securityGroup == null)
            {
                return NotFound();
            }

            context.SecurityGroups.Remove(securityGroup);
            context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Add a permission to a security group
        /// </summary>
        /// <param name="id"></param>
        /// <param name="permission_id"></param>
        /// <returns></returns>
        [HttpPost("{id}/permission/{permission_id}")]
        public ActionResult AddPermission( int id, int permission_id )
        {
            SecurityGroup? securityGroup = context.SecurityGroups.Find(id);

            if (securityGroup == null)
            {
                return NotFound("Security Group not found");
            }

            Permission? permission = context.Permissions.Find(permission_id);
            if (permission == null)
            {
                return NotFound("Permission not found");
            }

            if (context.SecurityGroupPermissions.Count(e => e.PermissionId == permission_id && e.SecurityGroupId == id) > 0)
            {
                return NotFound("Security Group already has that Permission");
            }

            context.SecurityGroupPermissions.Add(new SecurityGroupPermission()
            {
                PermissionId = permission_id,
                SecurityGroupId = id
            });

            context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Remove a permission from a security group
        /// </summary>
        /// <param name="id"></param>
        /// <param name="permission_id"></param>
        /// <returns></returns>
        [HttpDelete("{id}/permission/{permission_id}")]
        public ActionResult RemovePermission(int id, int permission_id)
        {
            SecurityGroup? securityGroup = context.SecurityGroups.Find(id);

            if (securityGroup == null)
            {
                return NotFound("Security Group not found");
            }

            Permission? permission = context.Permissions.Find(permission_id);
            if (permission == null)
            {
                return NotFound("Permission not found");
            }

            SecurityGroupPermission? link = context.SecurityGroupPermissions.FirstOrDefault(e => e.PermissionId == permission_id && e.SecurityGroupId == id);
            if (link == null)
            {
                return NotFound("Security Group does not have that Permission");
            }

            if (context.Entry(link).State == EntityState.Detached)
            {
                context.SecurityGroupPermissions.Attach(link);
            }

            context.SecurityGroupPermissions.Remove(link);
            context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Assign access to room for security group
        /// </summary>
        /// <param name="id"></param>
        /// <param name="room_id"></param>
        /// <param name="timeLimits"></param>
        /// <returns></returns>
        [HttpPost("{id}/room/{room_id}")]
        public ActionResult AddRoom(int id, int room_id, [FromBody] List<TimeLimit>? timeLimits )
        {
            SecurityGroup? securityGroup = context.SecurityGroups.Find(id);

            if (securityGroup == null)
            {
                return NotFound("Security Group not found");
            }

            Room? room = context.Rooms.Find(room_id);
            if (room == null)
            {
                return NotFound("Room not found");
            }

            if (context.SecurityGroupRooms.Count(e => e.RoomId == room_id && e.SecurityGroupId == id) > 0)
            {
                return NotFound("Security Group already has that Room");
            }

            context.SecurityGroupRooms.Add(new SecurityGroupRoom()
            {
                RoomId = room_id,
                SecurityGroupId = id,
                TimeLimits = timeLimits != null ? timeLimits : new List<TimeLimit>()
            });

            context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Edit access to room for security group
        /// </summary>
        /// <param name="id"></param>
        /// <param name="room_link_id"></param>
        /// <param name="timeLimits"></param>
        /// <returns></returns>
        [HttpPut("{id}/room_link/{room_link_id}")]
        public ActionResult EditRoom(int id, int room_link_id, [FromBody] List<TimeLimit>? timeLimits)
        {
            SecurityGroup? securityGroup = context.SecurityGroups.Find(id);

            if (securityGroup == null)
            {
                return NotFound("Security Group not found");
            }

            SecurityGroupRoom? link = context.SecurityGroupRooms.FirstOrDefault(e => e.Id == room_link_id && e.SecurityGroupId == id);
            if (link == null)
            {
                return NotFound("Security Group does not have that Room");
            }

            List<int> Ids = context.TimeLimits.Where(e => e.SecurityGroupRoomId == link.Id).Select(e => e.Id).ToList();

            if (timeLimits != null)
            {
                foreach (var item in timeLimits)
                {
                    if(Ids.Contains(item.Id))
                    {
                        Ids.Remove(item.Id);
                    }

                    item.SecurityGroupRoomId = link.Id;
                    context.Update(item);
                }
            }

            if(Ids.Count > 0)
            {
                context.TimeLimits.AsQueryable().Where(e => Ids.Contains(e.Id)).ExecuteDelete();
            }

            context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Remove access to room for security group
        /// </summary>
        /// <param name="id"></param>
        /// <param name="room_id"></param>
        /// <returns></returns>
        [HttpDelete("{id}/room/{room_id}")]
        public ActionResult RemoveRoom(int id, int room_id)
        {
            SecurityGroup? securityGroup = context.SecurityGroups.Find(id);

            if (securityGroup == null)
            {
                return NotFound("Security Group not found");
            }

            Room? room = context.Rooms.Find(room_id);
            if (room == null)
            {
                return NotFound("Room not found");
            }

            SecurityGroupRoom? link = context.SecurityGroupRooms
                .Include(e => e.TimeLimits)
                    .ThenInclude(e => e.Weeks)
                        .ThenInclude(e => e.Days)
                            .ThenInclude(e => e.Times)
                .FirstOrDefault(e => e.RoomId == room_id && e.SecurityGroupId == id);
            if (link == null)
            {
                return NotFound("Security Group does not have that Room");
            }

            if (context.Entry(link).State == EntityState.Detached)
            {
                context.Attach(link);
            }

            context.Remove(link);
            context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Edit time limits for a security group
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timeLimits"></param>
        /// <returns></returns>
        [HttpPut("{id}/timelimits")]
        public ActionResult TimeLimits(int id, [FromBody] List<TimeLimit>? timeLimits)
        {
            SecurityGroup? securityGroup = context.SecurityGroups.Find(id);

            if (securityGroup == null)
            {
                return NotFound("Security Group not found");
            }

            List<int> Ids = context.TimeLimits.Where(e => e.SecurityGroupId == id).Select(e => e.Id).ToList();

            if (timeLimits != null)
            {
                foreach (var item in timeLimits)
                {
                    if (Ids.Contains(item.Id))
                    {
                        Ids.Remove(item.Id);
                    }

                    item.SecurityGroupId = id;
                    context.Update(item);
                }
            }

            if (Ids.Count > 0)
            {
                context.TimeLimits.AsQueryable().Where(e => Ids.Contains(e.Id)).ExecuteDelete();
            }

            context.SaveChanges();

            return Ok();
        }
    }
}

