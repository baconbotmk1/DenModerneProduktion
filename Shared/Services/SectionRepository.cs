using System;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Shared.Services
{
    public class SectionRepository : RepositoryBase, IRepository<Section>
    {
        public SectionRepository(DataContext context) : base(context)
        {
        }

        public IEnumerable<Section> Get()
        {
            return context.Sections
                .Include(e => e.Building)
                .ToList();
        }

        public void Insert(Section section )
        {
            context.Sections.Add(section);
        }

        public Section? GetById( int id )
        {
            return context.Sections.Find(id);
        }

        public void Update(Section section)
        {
            

            context.Entry(section).State = EntityState.Modified;
        }

        public bool Delete( int id )
        {
            Section? section = context.Sections.Find(id);

            if(section == null)
            {
                return false;
            }

            context.Sections.Remove(section);

            return true;
        }
    }
}

