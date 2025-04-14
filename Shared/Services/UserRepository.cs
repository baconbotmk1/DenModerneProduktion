using System;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Shared.Services
{
    public class UserRepository : RepositoryBase, IRepository<User>
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        public IEnumerable<User> Get()
        {
            return context.Users
                .Include(e => e.AccessCards)
                .Include(e => e.UserSecurityGroups)
                .ThenInclude(e => e.SecurityGroup)
                .ThenInclude(e => e.SecurityGroupPermissions)
                .ThenInclude(e => e.Permission)
                .ToList();
        }

        public void Insert( User user )
        {
            context.Users.Add(user);
        }

        public User? GetById( int id )
        {
            return context.Users.Find(id);
        }

        public void Update(User user)
        {
            context.Entry(user).State = EntityState.Modified;
        }

        public bool Delete( int id )
        {
            User? user = context.Users.Find(id);

            if(user == null)
            {
                return false;
            }

            context.Users.Remove(user);

            return true;
        }
    }
}

