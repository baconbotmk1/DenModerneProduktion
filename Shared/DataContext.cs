using System;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Pomelo.EntityFrameworkCore.MySql;
using Shared.Models.Assignables.TimeLimit;
using Shared.Models.Assignables.LimitValues;

namespace Shared
{
    public class DataContext : DbContext
    {
        public DbSet<AccessCard> AccessCards { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Cadastre> Cadastres { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceData> DeviceDatas { get; set; }
        public DbSet<DeviceDataLimitValue> DeviceDataLimit { get; set; }
        public DbSet<DeviceMqttMap> DeviceMqttMaps { get; set; }
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
        public DbSet<UnknownMqttDevices> UnknownMqttDevices { get; set; }


        public string DbPath { get; }
        public DataContext()        
        {
            //var folder = Environment.SpecialFolder.MyDocuments;
            //var path = Environment.GetFolderPath(folder);
            //path = Path.Combine(path, "DenModerneProduktion");
            //if (!Directory.Exists(path))
            //{
            //    System.IO.Directory.CreateDirectory(path);
            //}
            //DbPath = System.IO.Path.Join(path, "data.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 41));
            //options.UseMySql("Server=127.0.0.1;Port=3308;Database=denmoderneproduktion;User=root;Password=pass", serverVersion);
            options.UseMySql("Server=5.75.144.48;Port=3389;Database=main;User=mysql;Password=DMPjs", serverVersion);
        }//options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeLimitAssignment>()
                .HasDiscriminator<string>("type")
                .HasValue<SecurityGroupDeviceTL>("secgrp_device")
                .HasValue<SecurityGroupRoomTL>("secgrp_room")
                .HasValue<SecurityGroupSectionTL>("secgrp_section")
                .HasValue<UserRoomTL>("user_room")
                .HasValue<UserSectionTL>("user_section")
                .HasValue<UserDeviceTL>("user_device");

            modelBuilder.Entity<LimitValueAssignment>()
                .HasDiscriminator<string>("type")
                .HasValue<RoomLV>("room")
                .HasValue<SectionLV>("section")
                .HasValue<BuildingLV>("building")
                .HasValue<CadastreLV>("cadastre");
        }
    }
}

