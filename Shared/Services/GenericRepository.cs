using System;
using Microsoft.EntityFrameworkCore;

namespace Shared.Services
{
    public class GenericRepository<T> : RepositoryBase, IRepository<T> where T : class
    {
        protected DbSet<T> dbSet;

        public GenericRepository(DataContext context) : base(context)
        {
            dbSet = context.Set<T>();
        }

        public virtual IEnumerable<T> Get()
        {
            return dbSet.ToList();
        }

        public virtual void Insert( T item )
        {
            dbSet.Add(item);
        }

        public virtual T? GetById( int id )
        {
            return dbSet.Find(id);
        }

        public virtual void Update(T item)
        {
            dbSet.Attach(item);
            context.Entry(item).State = EntityState.Modified;
        }

        public virtual bool Delete( int id )
        {
            T? entityToDelete = dbSet.Find(id);

            if (entityToDelete == null) return false;

            Delete(entityToDelete);

            return true;
        }

        public virtual void Delete(T item)
        {
            if (context.Entry(item).State == EntityState.Detached)
            {
                dbSet.Attach(item);
            }

            dbSet.Remove(item);
        }
    }
}

