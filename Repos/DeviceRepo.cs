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
            Device device = _context.Devices.Find(deviceId);
            return device;
        }

        public Device GetWithReadings(string deviceId)
        {
            Device device = _context.Devices.Include(d => d.Readings)
                .SingleOrDefault(d => deviceId.Equals(d.Id));
            return device;
        }

        public void Add(Device device)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Device device, string userId)
        {
            throw new System.NotImplementedException();
        }

        public bool CheckHostnameExists(string hostname, string userId)
        {
            return _context.Devices.Any(d => userId.Equals(d.User.UserId) && hostname.Equals(d.Hostname));
        }

        public List<Device> GetDevices(string userId)
        {
            var list = _context.Devices.Where(d => userId.Equals(d.User.UserId))
                .OrderBy(d => d.Hostname)
                .ToList();
            return list;
        }

        public List<Device> GetDevicesWithReadings(string userId)
        {
            var list = _context.Devices.Where(d => userId.Equals(d.User.UserId))
                .Include(d=>d.Readings)
                .OrderBy(d => d.Hostname)
                .ToList();
            return list;
        }
    }
}
