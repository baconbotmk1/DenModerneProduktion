using System;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Shared.Services
{
    public class BuildingRepository : RepositoryBase, IRepository<Building>
    {
        public BuildingRepository(DataContext context) : base(context)
        {

        }

        public IEnumerable<Building> Get()
        {
            return context.Buildings.ToList();
        }

        public void Insert(Building item )
        {
            context.Buildings.Add(item);
        }

        public Building? GetById( int id )
        {
            return context.Buildings.Find(id);
        }

        public void Update(Building item)
        {
            context.Buildings.Attach(item);
            context.Entry(item).State = EntityState.Modified;
        }

        public bool Delete( int id )
        {
            Building? entityToDelete = context.Buildings.Find(id);

            if (entityToDelete == null) return false;

            Delete(entityToDelete);

            return true;
        }

        public virtual void Delete(Building item)
        {
            if (context.Entry(item).State == EntityState.Detached)
            {
                context.Buildings.Attach(item);
            }

            context.Buildings.Remove(item);
        }
    }
}

