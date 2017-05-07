using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZRdotnetcore.Models;
using ZRdotnetcore.Models.DeviceViewModels;
using ZRdotnetcore.Repos.Interfaces;

namespace ZRdotnetcore.Controllers
{
    [Authorize]
    public class DeviceController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly IDeviceRepo _deviceRepo;
        private readonly IUserRepo _userRepo;

        public DeviceController(
            UserManager<ApplicationUser> userManager,
            ILoggerFactory loggerFactory,
            IDeviceRepo deviceRepo,
            IUserRepo userRepo)
        {
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<ManageController>();
            _deviceRepo = deviceRepo;
            _userRepo = userRepo;
        }

        //
        // GET: /Device/Index
        [HttpGet]
        public IActionResult Index(DeviceMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == DeviceMessageId.AddedDeviceSuccess ? "Device has been added successfully."
                : message == DeviceMessageId.UpdatedDeviceSuccess ? "Device has been updated successfully."
                : message == DeviceMessageId.Error ? "An error has occurred."
                : "";
            var devices = _deviceRepo.GetDevices(GetCurrentUserAsync().Result.Id);
            return View(devices);
        }

        //
        // GET: /Device/Add
        [HttpGet]
        public IActionResult Add()
        {
            var model = new AddDeviceViewModel { DeviceTypeList = _deviceRepo.GetDeviceTypesAll() };
            return View(model);
        }

        //
        // POST: /Device/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(AddDeviceViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (_deviceRepo.CheckHostnameExists(model.Hostname, GetCurrentUserAsync().Result.Id))
            {
                ModelState.AddModelError(string.Empty, "Hostname already exists in your devices.");
            }
            else
            {
                DeviceType deviceType = _deviceRepo.GetDeviceType(model.DeviceType);

                var device = new Device
                {
                    Id = Guid.NewGuid().ToString(),
                    User = _userRepo.Get(GetCurrentUserAsync().Result.Id),
                    AddedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    DeviceType = deviceType,
                    Hostname = model.Hostname
                };

                _deviceRepo.Add(device);
                _logger.LogInformation(3, "User created a new device.");
                return RedirectToAction(nameof(Index), new { Message = DeviceMessageId.AddedDeviceSuccess });
            }
            return View(model);
        }

        //
        // GET: /Device/Edit
        [HttpGet]
        public IActionResult Edit(string deviceId)
        {
            Device device = _deviceRepo.Get(deviceId);
            return View(device);
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