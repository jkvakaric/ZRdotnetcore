using System.Collections.Generic;
using ZRdotnetcore.Models;

namespace ZRdotnetcore.Repos.Interfaces
{
    public interface IDeviceRepo
    {
        Device Get(string deviceId);
        Device GetWithReadings(string deviceId);
        void Add(Device device);
        void Update(Device device);
        void Delete(Device device);
        bool CheckHostnameExists(string hostname, string userId);
        List<Device> GetDevices(string userId);
        List<Device> GetDevicesWithReadings(string userId);
        List<DeviceType> GetDeviceTypesAll();
        DeviceType GetDeviceType(string deviceTypeId);
    }
}