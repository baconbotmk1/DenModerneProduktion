using System;
using Microsoft.EntityFrameworkCore;

namespace Shared.Services
{
	public abstract class RepositoryBase : IDisposable
    {
        protected DataContext context;

        public RepositoryBase(DataContext context)
        {
            this.context = context;
        }

        protected bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public void Save()
        {
            context.SaveChanges();
        }
    }
}

