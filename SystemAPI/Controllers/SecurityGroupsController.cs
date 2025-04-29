using System.Diagnostics;
using Shared.DTOs.SecurityGroup;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/security_groups")]
    public class SecurityGroupsController : Controller, IDisposable
    {
        protected DataContext context;
        public SecurityGroupsController(DataContext Context)
        {
            context = Context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SecurityGroup>> Get()
        {
            try
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
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return StatusCode(500, "Internal Error");
            }
        }

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


        [HttpPost]
        [Consumes("application/json")]
        public ActionResult<SecurityGroup> Post([FromBody]CreateSecurityGroupDTO data)
        {
            SecurityGroup group = data.Adapt<SecurityGroup>();

            context.SecurityGroups.Add(group);
            context.SaveChanges();

            return Ok(group);
        }

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


        protected override void Dispose(bool disposing)
        {
            context.Dispose();

            base.Dispose(disposing);
        }
    }
}

