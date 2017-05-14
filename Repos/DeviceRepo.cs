using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ZRdotnetcore.Data;
using ZRdotnetcore.Models;
using ZRdotnetcore.Repos.Interfaces;

namespace ZRdotnetcore.Repos
{
    public class DeviceRepo : IDeviceRepo
    {
        private readonly YoctoDbContext _context;

        public DeviceRepo(YoctoDbContext context)
        {
            _context = context;
        }

        public Device Get(string deviceId)
        {
            Device device = _context.Devices
                .Include(d => d.User)
                .Include(d => d.DeviceType)
                .Include(d => d.ActiveReadings)
                    .ThenInclude(ar => ar.ReadingType)
                .SingleOrDefault(d => deviceId.Equals(d.Id));
            return device;
        }

        public Device GetWithReadings(string deviceId)
        {
            Device device = _context.Devices
                .Include(d => d.User)
                .Include(d => d.ActiveReadings)
                .Include(d => d.DeviceType)
                .Include(d => d.Readings)
                    .ThenInclude(r => r.ReadingType)
                .SingleOrDefault(d => deviceId.Equals(d.Id));
            return device;
        }

        public void Add(Device device)
        {
            _context.Devices.Add(device);
            _context.SaveChanges();
        }

        public void Update(Device device)
        {
            _context.Entry(device).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Device device)
        {
            _context.Devices.Remove(device);
            _context.SaveChanges();
        }

        public bool CheckHostnameExists(string hostname, string userId)
        {
            return _context.Devices.Any(d => userId.Equals(d.User.UserId) && hostname.Equals(d.Hostname));
        }

        public List<Device> GetDevices(string userId)
        {
            var list = _context.Devices.Where(d => userId.Equals(d.User.UserId))
                .Include(d => d.User)
                .Include(d => d.ActiveReadings)
                .Include(d => d.DeviceType)
                .OrderBy(d => d.Hostname)
                .ToList();
            return list;
        }

        public List<Device> GetDevicesWithReadings(string userId)
        {
            var list = _context.Devices.Where(d => userId.Equals(d.User.UserId))
                .Include(d => d.User)
                .Include(d => d.ActiveReadings)
                .Include(d => d.DeviceType)
                .Include(d => d.Readings)
                .OrderBy(d => d.Hostname)
                .ToList();
            return list;
        }

        public List<DeviceType> GetDeviceTypesAll()
        {
            var list = _context.DeviceTypes.ToList();
            return list;
        }

        public DeviceType GetDeviceType(string deviceTypeId)
        {
            DeviceType device = _context.DeviceTypes.Find(deviceTypeId);
            return device;
        }
    }
}
