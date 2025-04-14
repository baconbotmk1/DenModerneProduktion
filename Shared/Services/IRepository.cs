using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface IRepository<T> where T : class
    {
        public IEnumerable<T> Get();

        public void Insert(T data);

        public T? GetById(int id);

        public void Update(T data);

        public bool Delete(int id);

        public void Dispose();
        public void Save();
    }
}
