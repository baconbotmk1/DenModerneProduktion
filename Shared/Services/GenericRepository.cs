using System;
using Microsoft.EntityFrameworkCore;

namespace Shared.Services
{
    public class GenericRepository<T> : RepositoryBase where T : class
    {
        protected DbSet<T> dbSet;

        public GenericRepository(DataContext context) : base(context)
        {
            dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public void Insert( T item )
        {
            dbSet.Add(item);
        }

        public T? GetById( int id )
        {
            return dbSet.Find(id);
        }

        public void Update(T item)
        {
            dbSet.Attach(item);
            context.Entry(item).State = EntityState.Modified;
        }

        public bool Delete( int id )
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

