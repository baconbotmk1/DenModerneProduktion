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
                    .Include(e => e.SecurityGroupPermissions)
                    .ThenInclude(e => e.Permission)
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
            SecurityGroup? securityGroup = context.SecurityGroups.Find(id);

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

        [HttpPut("{id}/add_permission/{permission_id}")]
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


        [HttpDelete("{id}/remove_permission/{permission_id}")]
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

        protected override void Dispose(bool disposing)
        {
            context.Dispose();

            base.Dispose(disposing);
        }
    }
}

