using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZRdotnetcore.Models;
using ZRdotnetcore.Repos.Interfaces;

namespace ZRdotnetcore.Controllers
{
    public class DeviceController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly IDeviceRepo _deviceRepo;

        public DeviceController(
            UserManager<ApplicationUser> userManager,
            ILoggerFactory loggerFactory,
            IDeviceRepo deviceRepo)
        {
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<ManageController>();
            _deviceRepo = deviceRepo;
        }

        public IActionResult Index()
        {
            var devices = _deviceRepo.GetDevices(GetCurrentUserAsync().Result.Id);
            return View(devices);
        }

        // u kontroleru kad se vrati device provjeriti jel pripada tom useru

        #region Helpers

        public enum DeviceMessageId
        {
            AddedDeviceSuccess,
            UpdatedDeviceSuccess,
            Error
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}