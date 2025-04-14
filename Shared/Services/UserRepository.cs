using System;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Shared.Services
{
    public class UserRepository : RepositoryBase
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        public IEnumerable<User> GetUsers()
        {
            return context.Users.ToList();
        }

        public void InsertUser( User user )
        {
            context.Users.Add(user);
        }

        public User? GetUserById( int id )
        {
            return context.Users.Find(id);
        }

        public void UpdateUser(User user)
        {
            context.Entry(user).State = EntityState.Modified;
        }

        public bool DeleteUser( int id )
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

