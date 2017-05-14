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
        private readonly IReadingsRepo _readingsRepo;

        public DeviceController(
            UserManager<ApplicationUser> userManager,
            ILoggerFactory loggerFactory,
            IDeviceRepo deviceRepo,
            IUserRepo userRepo,
            IReadingsRepo readingsRepo)
        {
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<ManageController>();
            _deviceRepo = deviceRepo;
            _userRepo = userRepo;
            _readingsRepo = readingsRepo;
        }

        //
        // GET: /Device/Index
        [HttpGet]
        public IActionResult Index(DeviceMessageId? dmessage = null)
        {
            ViewData["StatusMessage"] =
                dmessage == DeviceMessageId.AddDeviceSuccess ? "Device has been added successfully."
                : dmessage == DeviceMessageId.AddDeviceFailed ? "Device could not be added."
                : dmessage == DeviceMessageId.DeviceNotFound ? "Device could not be found."
                : dmessage == DeviceMessageId.NotAuthorized ? "You are not authorized to perform this action."
                : dmessage == DeviceMessageId.UpdateDeviceSuccess ? "Device has been updated successfully."
                : dmessage == DeviceMessageId.UpdateDeviceFailed ? "Device could not be updated."
                : dmessage == DeviceMessageId.DeleteDeviceSuccess ? "Device has been deleted successfully."
                : dmessage == DeviceMessageId.DeleteDeviceFailed ? "Device could not be deleted."
                : dmessage == DeviceMessageId.HostnameAlreadyExists ? "This hostname already exists in your collection."
                : dmessage == DeviceMessageId.ValidationFailed ? "Input form validation failed. Try again."
                : dmessage == DeviceMessageId.Error ? "An error has occurred."
                : "";
            var devices = _deviceRepo.GetDevices(GetCurrentUserAsync().Result.Id);
            return View(devices);
        }

        //
        // GET: /Device/Add
        [HttpGet]
        public IActionResult Add()
        {
            var model = new DeviceAddViewModel { DeviceTypeList = _deviceRepo.GetDeviceTypesAll() };
            return PartialView(model);
        }

        //
        // POST: /Device/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(DeviceAddViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.ValidationFailed });

            if (_deviceRepo.CheckHostnameExists(model.Hostname, GetCurrentUserAsync().Result.Id))
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.HostnameAlreadyExists });

            try
            {
                var device = new Device
                {
                    Id = Guid.NewGuid().ToString(),
                    User = _userRepo.Get(GetCurrentUserAsync().Result.Id),
                    AddedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    DeviceType = _deviceRepo.GetDeviceType(model.DeviceType),
                    Hostname = model.Hostname
                };

                _deviceRepo.Add(device);
                _logger.LogInformation("User created a new device.");
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.AddDeviceSuccess });
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return RedirectToAction("Index", new { dmessage = DeviceMessageId.AddDeviceFailed });
            }
        }

        //
        // GET: /Device/Edit
        [HttpGet]
        public IActionResult Edit(string deviceId)
        {
            if (deviceId == null)
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.DeviceNotFound });

            Device device = _deviceRepo.Get(deviceId);
            if (device == null)
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.DeviceNotFound });

            if (!device.User.UserId.Equals(GetCurrentUserAsync().Result.Id))
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.NotAuthorized });

            var model = new DeviceEditViewModel
            {
                Id = device.Id,
                Hostname = device.Hostname,
                DeviceType = device.DeviceType.Name,
                DeviceTypeList = _deviceRepo.GetDeviceTypesAll()
            };

            return PartialView(model);
        }

        //
        // POST: /Device/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DeviceEditViewModel model)
        {
            if (model.Id == null)
                return RedirectToAction("Index", new { dmessage = DeviceMessageId.DeviceNotFound });

            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.ValidationFailed });

            try
            {
                Device device = _deviceRepo.Get(model.Id);

                var authUserId = GetCurrentUserAsync().Result.Id;
                if (device.Hostname != model.Hostname)
                    if (_deviceRepo.CheckHostnameExists(model.Hostname, authUserId))
                        return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.HostnameAlreadyExists });

                if (!device.User.UserId.Equals(authUserId))
                    return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.NotAuthorized });

                device.Hostname = model.Hostname;
                device.DeviceType = _deviceRepo.GetDeviceType(model.DeviceType);
                device.UpdatedOn = DateTime.Now;

                _deviceRepo.Update(device);
                _logger.LogInformation("User updated one device.");
                return RedirectToAction("Index", new { dmessage = DeviceMessageId.UpdateDeviceSuccess });
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return RedirectToAction("Index", new { dmessage = DeviceMessageId.UpdateDeviceFailed });
            }
        }

        //
        // GET: /Device/Delete
        [HttpGet]
        public IActionResult Delete(string deviceId)
        {
            if (deviceId == null)
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.DeviceNotFound });

            Device device = _deviceRepo.Get(deviceId);
            if (device == null)
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.DeviceNotFound });

            if (!device.User.UserId.Equals(GetCurrentUserAsync().Result.Id))
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.NotAuthorized });

            var model = new DeviceDeleteViewModel
            {
                Id = device.Id,
                Hostname = device.Hostname
            };

            return PartialView(model);
        }

        //
        // POST: /Device/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(DeviceDeleteViewModel model)
        {
            if (model.Id == null)
                return RedirectToAction("Index", new { dmessage = DeviceMessageId.DeviceNotFound });

            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.ValidationFailed });

            try
            {
                Device device = _deviceRepo.Get(model.Id);

                string authUserId = GetCurrentUserAsync().Result.Id;
                if (!device.User.UserId.Equals(authUserId))
                    return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.NotAuthorized });

                _deviceRepo.Delete(device);
                _logger.LogInformation("User deleted one device.");
                return RedirectToAction("Index", new { dmessage = DeviceMessageId.DeleteDeviceSuccess });
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return RedirectToAction("Index", new { dmessage = DeviceMessageId.DeleteDeviceFailed });
            }
        }

        //
        // GET: /Device/Info
        [HttpGet]
        public IActionResult Info(string deviceId)
        {
            if (deviceId == null)
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.DeviceNotFound });

            Device device = _deviceRepo.Get(deviceId);
            if (device == null)
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.DeviceNotFound });

            if (!device.User.UserId.Equals(GetCurrentUserAsync().Result.Id))
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.NotAuthorized });

            Device model = _deviceRepo.Get(deviceId);

            return PartialView(model);
        }

        //
        // GET: /Device/Manage
        [HttpGet]
        public IActionResult Manage(string deviceId, ReadingController.ActiveReadingMessageId? armessage = null, ReadingController.ReadingMessageId? rmessage = null)
        {
            ViewData["StatusMessage"] =
                armessage == ReadingController.ActiveReadingMessageId.AddActiveReadingSuccess ? "Active Reading has been added successfully."
                : armessage == ReadingController.ActiveReadingMessageId.AddActiveReadingFailed ? "Active Reading could not be added."
                : armessage == ReadingController.ActiveReadingMessageId.ActiveReadingNotFound ? "Active Reading could not be found."
                : armessage == ReadingController.ActiveReadingMessageId.NotAuthorized ? "You are not authorized to perform this action."
                : armessage == ReadingController.ActiveReadingMessageId.DeleteActiveReadingSuccess ? "Active Reading has been deleted successfully."
                : armessage == ReadingController.ActiveReadingMessageId.DeleteActiveReadingFailed ? "Active Reading could not be deleted."
                : armessage == ReadingController.ActiveReadingMessageId.ReadingNameAlreadyExists ? "This (Active) Reading Name already exists in your collection."
                : armessage == ReadingController.ActiveReadingMessageId.ValidationFailed ? "Input form validation failed. Try again."
                : armessage == ReadingController.ActiveReadingMessageId.Error ? "An error has occurred."
                : armessage == ReadingController.ActiveReadingMessageId.DeviceNotFound ? "Device could not be found."
                : "";
            if (ViewData["StatusMessage"].Equals(""))
            {
                ViewData["StatusMessage"] =
                    rmessage == ReadingController.ReadingMessageId.ReadingNotFound ? "Reading could not be found."
                    : rmessage == ReadingController.ReadingMessageId.NotAuthorized ? "You are not authorized to perform this action."
                    : rmessage == ReadingController.ReadingMessageId.DeleteReadingSuccess ? "Reading has been deleted successfully."
                    : rmessage == ReadingController.ReadingMessageId.DeleteReadingFailed ? "Reading could not be deleted."
                    : rmessage == ReadingController.ReadingMessageId.ValidationFailed ? "Input form validation failed. Try again."
                    : rmessage == ReadingController.ReadingMessageId.Error ? "An error has occurred."
                    : rmessage == ReadingController.ReadingMessageId.DeviceNotFound ? "Device could not be found."
                    : "";
            }

            if (deviceId == null)
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.DeviceNotFound });

            Device device = _deviceRepo.Get(deviceId);
            if (device == null)
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.DeviceNotFound });

            if (!device.User.UserId.Equals(GetCurrentUserAsync().Result.Id))
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.NotAuthorized });

            Device model = _deviceRepo.Get(deviceId);

            return View(model);
        }

        //
        // GET: /Device/Readings
        [HttpGet]
        public IActionResult Readings(string deviceId, ReadingController.ReadingMessageId? rmessage = null)
        {
            ViewData["StatusMessage"] =
                rmessage == ReadingController.ReadingMessageId.ReadingNotFound ? "Reading could not be found."
                : rmessage == ReadingController.ReadingMessageId.NotAuthorized ? "You are not authorized to perform this action."
                : rmessage == ReadingController.ReadingMessageId.DeleteReadingSuccess ? "Reading has been deleted successfully."
                : rmessage == ReadingController.ReadingMessageId.DeleteReadingFailed ? "Reading could not be deleted."
                : rmessage == ReadingController.ReadingMessageId.ValidationFailed ? "Input form validation failed. Try again."
                : rmessage == ReadingController.ReadingMessageId.Error ? "An error has occurred."
                : rmessage == ReadingController.ReadingMessageId.DeviceNotFound ? "Device could not be found."
                : "";

            if (deviceId == null)
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.DeviceNotFound });

            Device device = _deviceRepo.Get(deviceId);
            if (device == null)
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.DeviceNotFound });

            if (!device.User.UserId.Equals(GetCurrentUserAsync().Result.Id))
                return RedirectToAction(nameof(Index), new { dmessage = DeviceMessageId.NotAuthorized });

            Device model = _deviceRepo.GetWithReadings(deviceId);

            return View(model);
        }

        #region Helpers

        public enum DeviceMessageId
        {
            AddDeviceSuccess,
            AddDeviceFailed,
            DeviceNotFound,
            NotAuthorized,
            UpdateDeviceSuccess,
            UpdateDeviceFailed,
            DeleteDeviceSuccess,
            DeleteDeviceFailed,
            HostnameAlreadyExists,
            ValidationFailed,
            Error
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}