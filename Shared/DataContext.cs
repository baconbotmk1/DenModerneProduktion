using System;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Shared
{
	public class DataContext : DbContext
	{
		public DbSet<AccessCard> AccessCards { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Cadastre> Cadastres { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceData> DeviceDatas { get; set; }
        public DbSet<DeviceDataType> DeviceDataTypes { get; set; }
        public DbSet<DeviceEvent> DeviceEvents { get; set; }
        public DbSet<DeviceEventType> DeviceEventTypes { get; set; }
        public DbSet<DeviceInfo> DeviceInfos { get; set; }
        public DbSet<DeviceInfoType> DeviceInfoTypes { get; set; }
        public DbSet<DeviceRecording> DeviceRecordings { get; set; }
        public DbSet<DeviceSharedCategory> DeviceSharedCategories { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<SecurityGroup> SecurityGroups { get; set; }
        public DbSet<SecurityGroupDevice> SecurityGroupDevices { get; set; }
        public DbSet<SecurityGroupRoom> SecurityGroupRooms { get; set; }
        public DbSet<SecurityGroupSection> SecurityGroupSections { get; set; }
        public DbSet<SecurityGroupPermission> SecurityGroupPermissions { get; set; }
        public DbSet<TimeLimit> TimeLimits { get; set; }
        public DbSet<TimeLimitWeek> TimeLimitWeeks { get; set; }
        public DbSet<TimeLimitWeekDay> TimeLimitWeekDays { get; set; }
        public DbSet<TimeLimitWeekDayTime> TimeLimitWeekDayTimes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<UserRoom> UserRooms { get; set; }
        public DbSet<UserSection> UserSections { get; set; }
        public DbSet<UserSecurityGroup> UserSecurityGroups { get; set; }


        public string DbPath { get; }
        public DataContext()
	    {
            var folder = Environment.SpecialFolder.MyDocuments;
            var path = Environment.GetFolderPath(folder);
            path = Path.Combine(path, "DenModerneProduktion");
            if (!Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            DbPath = System.IO.Path.Join(path, "data.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}

