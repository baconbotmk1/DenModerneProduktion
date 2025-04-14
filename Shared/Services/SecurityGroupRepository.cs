using System;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Shared.Services
{
    public class SecurityGroupRepository : RepositoryBase, IRepository<SecurityGroup>
    {
        public SecurityGroupRepository(DataContext context) : base(context)
        {

        }

        public IEnumerable<SecurityGroup> Get()
        {
            return context.SecurityGroups.ToList();
        }

        public void Insert(SecurityGroup item )
        {
            context.SecurityGroups.Add(item);
        }

        public SecurityGroup? GetById( int id )
        {
            return context.SecurityGroups.Find(id);
        }

        public void Update(SecurityGroup item)
        {
            context.SecurityGroups.Attach(item);
            context.Entry(item).State = EntityState.Modified;
        }

        public bool Delete( int id )
        {
            SecurityGroup? entityToDelete = context.SecurityGroups.Find(id);

            if (entityToDelete == null) return false;

            Delete(entityToDelete);

            return true;
        }

        public virtual void Delete(SecurityGroup item)
        {
            if (context.Entry(item).State == EntityState.Detached)
            {
                context.SecurityGroups.Attach(item);
            }

            context.SecurityGroups.Remove(item);
        }
    }
}

