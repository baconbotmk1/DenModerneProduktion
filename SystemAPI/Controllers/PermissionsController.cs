using Shared.DTOs.Permission;
namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/permissions")]
    public class PermissionsController : GenericCRUDController<Permission, CreatePermissionDTO>
    {
        public PermissionsController(DataContext Context, GenericRepository<Permission>? DIrepository = null) : base(Context, DIrepository)
        {
        }
    }
}

