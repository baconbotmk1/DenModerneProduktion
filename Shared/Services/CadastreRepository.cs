using System;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Shared.Services
{
    public class CadastreRepository : RepositoryBase, IRepository<Cadastre>
    {
        public CadastreRepository(DataContext context) : base(context)
        {

        }

        public IEnumerable<Cadastre> Get()
        {
            return context.Cadastres.ToList();
        }

        public void Insert(Cadastre item )
        {
            context.Cadastres.Add(item);
        }

        public Cadastre? GetById( int id )
        {
            return context.Cadastres.Find(id);
        }

        public void Update(Cadastre item)
        {
            context.Cadastres.Attach(item);
            context.Entry(item).State = EntityState.Modified;
        }

        public bool Delete( int id )
        {
            Cadastre? entityToDelete = context.Cadastres.Find(id);

            if (entityToDelete == null) return false;

            Delete(entityToDelete);

            return true;
        }

        public virtual void Delete(Cadastre item)
        {
            if (context.Entry(item).State == EntityState.Detached)
            {
                context.Cadastres.Attach(item);
            }

            context.Cadastres.Remove(item);
        }
    }
}

