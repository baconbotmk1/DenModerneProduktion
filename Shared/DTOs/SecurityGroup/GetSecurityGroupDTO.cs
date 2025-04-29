using System;
using System.ComponentModel.DataAnnotations;
using Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.SecurityGroup
{
	public class GetSecurityGroupDTO : BaseModel
	{
        public string Name { get; set; }
        public string? Desc { get; set; }

        public IEnumerable<Permission> Permissions { get; set; } = new List<Permission>();
        public IEnumerable<User> Users { get; set; } = new List<User>();

        public class Permission
        {
            public int Id { get; set; }

            public string Slug { get; set; }
            public string Name { get; set; }
            public string Desc { get; set; }
        }

        public class User
        {
            public int Id { get; set; }

            public string Name { get; set; } = default!;
            public bool IsActive { get; set; } = false;

            public string Username { get; set; }
            public string? HashedPassword { get; set; }
            public string? Salt { get; set; }
            public string? ResetToken { get; set; }

            public string? ReferenceId { get; set; }
            public string? ReferenceType { get; set; }
        }
    }
}

