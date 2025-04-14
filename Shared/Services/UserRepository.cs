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

        }

        public User? GetUserById( int id )
        {
            return context.Users.Find(id);
        }
    }
}

