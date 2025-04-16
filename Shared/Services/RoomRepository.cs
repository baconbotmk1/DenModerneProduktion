using System;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Shared.Services
{
    public class RoomRepository : RepositoryBase, IRepository<Room>
    {
        public RoomRepository(DataContext context) : base(context)
        {
        }

        public IEnumerable<Room> Get()
        {
            return context.Rooms
                .Include(e => e.Section)
                .ToList();
        }

        public void Insert( Room room )
        {
            context.Rooms.Add(room);
        }

        public Room? GetById( int id )
        {
            return context.Rooms.Find(id);
        }

        public void Update(Room room)
        {
            

            context.Entry(room).State = EntityState.Modified;
        }

        public bool Delete( int id )
        {
            Room? room = context.Rooms.Find(id);

            if(room == null)
            {
                return false;
            }

            context.Rooms.Remove(room);

            return true;
        }
    }
}

