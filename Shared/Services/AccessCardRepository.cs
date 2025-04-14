using System;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Models;

namespace Shared.Services
{
    public class AccessCardRepository : GenericRepository<AccessCard>, IRepository<AccessCard>
    {
        public AccessCardRepository(DataContext context) : base(context)
        {
        }

        public IEnumerable<AccessCard> Get()
        {
            return dbSet
                .AsQueryable()
                .Include(e => e.User)
                .ToList();
        }

        public AccessCard? GetById(int id)
        {
            return dbSet
                .AsQueryable()
                .Include(e => e.User)
                .FirstOrDefault(e => e.Id == id);
        }
    }
}

