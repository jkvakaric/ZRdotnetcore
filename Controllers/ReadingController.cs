using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZRdotnetcore.Models;
using ZRdotnetcore.Models.ReadingViewModels;
using ZRdotnetcore.Repos.Interfaces;

namespace ZRdotnetcore.Controllers
{
    [Authorize]
    public class ReadingController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly IDeviceRepo _deviceRepo;
        private readonly IReadingsRepo _readingsRepo;

        public ReadingController(
            UserManager<ApplicationUser> userManager,
            ILoggerFactory loggerFactory,
            IDeviceRepo deviceRepo,
            IReadingsRepo readingsRepo)
        {
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<ManageController>();
            _deviceRepo = deviceRepo;
            _readingsRepo = readingsRepo;
        }

        //
        // GET: /Reading/AddActive
        [HttpGet]
        public IActionResult AddActive(string deviceId)
        {
            if (deviceId == null)
                return RedirectToAction("Manage", "Device", new { armessage = ActiveReadingMessageId.ActiveReadingNotFound });

            Device device = _deviceRepo.Get(deviceId);
            if (device == null)
                return RedirectToAction("Index", "Device", new { dmessage = DeviceController.DeviceMessageId.DeviceNotFound });

            ViewData["deviceName"] = device.Hostname;
            var model = new ActiveReadingAddViewModel
            {
                DeviceId = deviceId,
                ReadingTypeList = _readingsRepo.GetReadingTypesAll()
            };
            return PartialView(model);
        }

        //
        // POST: /Reading/AddActive
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddActive(ActiveReadingAddViewModel model)
        {
            if (model.DeviceId == null)
                return RedirectToAction("Index", "Device", new { armessage = ActiveReadingMessageId.DeviceNotFound });

            Device device = _deviceRepo.Get(model.DeviceId);
            if (device == null)
                return RedirectToAction("Index", "Device", new { armessage = ActiveReadingMessageId.DeviceNotFound });

            if (!ModelState.IsValid)
                return RedirectToAction("Manage", "Device", new { DeviceId = device.Id, armessage = ActiveReadingMessageId.ValidationFailed });

            string authUserId = GetCurrentUserAsync().Result.Id;
            if (!device.User.UserId.Equals(authUserId))
                return RedirectToAction("Manage", "Device", new { DeviceId = device.Id, armessage = ActiveReadingMessageId.NotAuthorized });

            if (_readingsRepo.CheckReadingNameAlreadyExists(model.Name, authUserId))
                return RedirectToAction("Manage", "Device", new { DeviceId = device.Id, armessage = ActiveReadingMessageId.ReadingNameAlreadyExists });

            try
            {
                var activeReading = new ActiveReading
                {
                    Id = Guid.NewGuid().ToString(),
                    Device = device,
                    ActiveSince = DateTime.Now,
                    Name = model.Name,
                    DataFilepath = model.DataFilepath,
                    ReadingType = _readingsRepo.GetReadingType(model.ReadingType),
                    Owner = device.User
                };

                _readingsRepo.AddActiveReading(activeReading);
                _logger.LogInformation("User activated a new reading.");
                return RedirectToAction("Manage", "Device", new { DeviceId = device.Id, armessage = ActiveReadingMessageId.AddActiveReadingSuccess });
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return RedirectToAction("Manage", "Device", new { DeviceId = device.Id, armessage = ActiveReadingMessageId.AddActiveReadingFailed });
            }
        }

        //
        // GET: /Reading/DeleteActive
        [HttpGet]
        public IActionResult DeleteActive(string deviceId, string readingId)
        {
            if (deviceId == null)
                return RedirectToAction("Index", "Device", new { armessage = ActiveReadingMessageId.DeviceNotFound });

            Device device = _deviceRepo.Get(deviceId);
            if (device == null)
                return RedirectToAction("Index", "Device", new { armessage = ActiveReadingMessageId.DeviceNotFound });

            if (readingId == null)
                return RedirectToAction("Manage", "Device", new { DeviceId = device.Id, armessage = ActiveReadingMessageId.ActiveReadingNotFound });

            ActiveReading reading = _readingsRepo.GetActiveReading(readingId);
            if (reading == null)
                return RedirectToAction("Manage", "Device", new { DeviceId = device.Id, armessage = ActiveReadingMessageId.ActiveReadingNotFound });

            if (!reading.Device.User.UserId.Equals(GetCurrentUserAsync().Result.Id))
                return RedirectToAction("Manage", "Device", new { DeviceId = device.Id, armessage = ActiveReadingMessageId.NotAuthorized });

            ViewData["deviceName"] = device.Hostname;
            var model = new ActiveReadingDeleteViewModel
            {
                Id = reading.Id,
                Name = reading.Name,
                DeviceId = device.Id
            };

            return PartialView(model);
        }

        //
        // POST: /Reading/DeleteActive
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteActive(ActiveReadingDeleteViewModel model)
        {
            if (model.DeviceId == null)
                return RedirectToAction("Index", "Device", new { armessage = ActiveReadingMessageId.DeviceNotFound });

            Device device = _deviceRepo.Get(model.DeviceId);
            if (device == null)
                return RedirectToAction("Index", "Device", new { armessage = ActiveReadingMessageId.DeviceNotFound });

            if (!ModelState.IsValid)
                return RedirectToAction("Manage", "Device", new { DeviceId = device.Id, armessage = ActiveReadingMessageId.ValidationFailed });

            if (model.Id == null)
                return RedirectToAction("Manage", "Device", new { DeviceId = device.Id, armessage = ActiveReadingMessageId.ActiveReadingNotFound });

            try
            {
                ActiveReading activeReading = _readingsRepo.GetActiveReading(model.Id);

                string authUserId = GetCurrentUserAsync().Result.Id;
                if (!activeReading.Device.User.UserId.Equals(authUserId))
                    return RedirectToAction("Manage", "Device", new { DeviceId = device.Id, armessage = ActiveReadingMessageId.NotAuthorized });

                _readingsRepo.DeleteActive(activeReading);
                _logger.LogInformation("User deleted one Active Reading.");
                return RedirectToAction("Manage", "Device", new { DeviceId = device.Id, armessage = ActiveReadingMessageId.DeleteActiveReadingSuccess });
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return RedirectToAction("Manage", "Device", new { DeviceId = device.Id, armessage = ActiveReadingMessageId.DeleteActiveReadingFailed });
            }
        }

        //
        // GET: /Reading/InfoActive
        [HttpGet]
        public IActionResult InfoActive(string deviceId, string readingId)
        {
            if (deviceId == null)
                return RedirectToAction("Index", "Device", new { armessage = ActiveReadingMessageId.DeviceNotFound });

            Device device = _deviceRepo.Get(deviceId);
            if (device == null)
                return RedirectToAction("Index", "Device", new { armessage = ActiveReadingMessageId.DeviceNotFound });

            if (readingId == null)
                return RedirectToAction("Manage", "Device", new { DeviceId = device.Id, armessage = ActiveReadingMessageId.ActiveReadingNotFound });

            ActiveReading model = _readingsRepo.GetActiveReadingWithReadings(readingId);
            if (model == null)
                return RedirectToAction("Manage", "Device", new { DeviceId = device.Id, armessage = ActiveReadingMessageId.ActiveReadingNotFound });

            if (!model.Device.User.UserId.Equals(GetCurrentUserAsync().Result.Id))
                return RedirectToAction("Manage", "Device", new { DeviceId = device.Id, armessage = ActiveReadingMessageId.NotAuthorized });

            return PartialView(model);
        }

        //
        // GET: /Reading/AllFromUser
        [HttpGet]
        public IActionResult AllFromUser(ReadingMessageId? rmessage = null)
        {
            ViewData["StatusMessage"] =
                rmessage == ReadingMessageId.ReadingNotFound ? "Reading could not be found."
                : rmessage == ReadingMessageId.NotAuthorized ? "You are not authorized to perform this action."
                : rmessage == ReadingMessageId.DeleteReadingSuccess ? "Reading has been deleted successfully."
                : rmessage == ReadingMessageId.DeleteReadingFailed ? "Reading could not be deleted."
                : rmessage == ReadingMessageId.ValidationFailed ? "Input form validation failed. Try again."
                : rmessage == ReadingMessageId.Error ? "An error has occurred."
                : rmessage == ReadingMessageId.DeviceNotFound ? "Device could not be found."
                : "";

            string authUserId = GetCurrentUserAsync().Result.Id;
            var model = _readingsRepo.GetAllFromUser(authUserId);
            return View(model);
        }

        //
        // GET: /Reading/DeleteFromDevice
        [HttpGet]
        public IActionResult DeleteFromDevice(string deviceId, string readingId)
        {
            if (deviceId == null)
                return RedirectToAction("Index", "Device", new { rmessage = ReadingMessageId.DeviceNotFound });

            Device device = _deviceRepo.Get(deviceId);
            if (device == null)
                return RedirectToAction("Index", "Device", new { rmessage = ReadingMessageId.DeviceNotFound });

            if (readingId == null)
                return RedirectToAction("Manage", "Device", new { DeviceId = device.Id, rmessage = ReadingMessageId.ReadingNotFound });

            Reading reading = _readingsRepo.GetReading(readingId);
            if (reading == null)
                return RedirectToAction("Manage", "Device", new { DeviceId = device.Id, rmessage = ReadingMessageId.ReadingNotFound });

            if (!reading.Owner.UserId.Equals(GetCurrentUserAsync().Result.Id))
                return RedirectToAction("Manage", "Device", new { DeviceId = device.Id, rmessage = ReadingMessageId.NotAuthorized });

            ViewData["deviceName"] = device.Hostname;
            var model = new ReadingDeleteFromDeviceViewModel
            {
                Id = reading.Id,
                Name = reading.Name,
                DeviceId = device.Id,
                ReadValue = reading.ReadValue,
                Timestamp = reading.Timestamp
            };

            ViewData["readingType"] = reading.ReadingType.Name;

            return PartialView(model);
        }

        //
        // POST: /Reading/DeleteFromDevice
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFromDevice(ReadingDeleteFromDeviceViewModel model)
        {
            if (model.DeviceId == null)
                return RedirectToAction("Index", "Device", new { rmessage = ReadingMessageId.DeviceNotFound });

            Device device = _deviceRepo.Get(model.DeviceId);
            if (device == null)
                return RedirectToAction("Index", "Device", new { rmessage = ReadingMessageId.DeviceNotFound });

            if (model.Id == null)
                return RedirectToAction("Readings", "Device", new { DeviceId = device.Id, rmessage = ReadingMessageId.ReadingNotFound });

            if (!ModelState.IsValid)
                return RedirectToAction("Readings", "Device", new { DeviceId = device.Id, rmessage = ReadingMessageId.ValidationFailed });

            try
            {
                Reading reading = _readingsRepo.GetReading(model.Id);

                string authUserId = GetCurrentUserAsync().Result.Id;
                if (!reading.Owner.UserId.Equals(authUserId))
                    return RedirectToAction("Readings", "Device", new { DeviceId = device.Id, rmessage = ReadingMessageId.NotAuthorized });

                _readingsRepo.Delete(reading);
                _logger.LogInformation("User deleted one Reading.");
                return RedirectToAction("Readings", "Device", new { DeviceId = device.Id, rmessage = ReadingMessageId.DeleteReadingSuccess });
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return RedirectToAction("Readings", "Device", new { DeviceId = device.Id, rmessage = ReadingMessageId.DeleteReadingFailed });
            }
        }

        //
        // GET: /Reading/DeleteFromAll
        [HttpGet]
        public IActionResult DeleteFromAll(string readingId)
        {
            if (readingId == null)
                return RedirectToAction("AllFromUser", "Reading", new { rmessage = ReadingMessageId.ReadingNotFound });

            Reading reading = _readingsRepo.GetReading(readingId);
            if (reading == null)
                return RedirectToAction("AllFromUser", "Reading", new { rmessage = ReadingMessageId.ReadingNotFound });

            if (!reading.Owner.UserId.Equals(GetCurrentUserAsync().Result.Id))
                return RedirectToAction("AllFromUser", "Reading", new { rmessage = ReadingMessageId.NotAuthorized });

            var model = new ReadingDeleteFromAllViewModel
            {
                Id = reading.Id,
                Name = reading.Name,
                ReadValue = reading.ReadValue,
                Timestamp = reading.Timestamp
            };

            ViewData["readingType"] = reading.ReadingType.Name;

            return PartialView(model);
        }

        //
        // POST: /Reading/DeleteFromAll
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFromAll(ReadingDeleteFromAllViewModel model)
        {
            if (model.Id == null)
                return RedirectToAction("AllFromUser", "Reading", new { rmessage = ReadingMessageId.ReadingNotFound });

            if (!ModelState.IsValid)
                return RedirectToAction("AllFromUser", "Reading", new { rmessage = ReadingMessageId.ValidationFailed });

            try
            {
                Reading reading = _readingsRepo.GetReading(model.Id);

                string authUserId = GetCurrentUserAsync().Result.Id;
                if (!reading.Owner.UserId.Equals(authUserId))
                    return RedirectToAction("AllFromUser", "Reading", new { rmessage = ReadingMessageId.NotAuthorized });

                _readingsRepo.Delete(reading);
                _logger.LogInformation("User deleted one Reading.");
                return RedirectToAction("AllFromUser", "Reading", new { rmessage = ReadingMessageId.DeleteReadingSuccess });
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return RedirectToAction("AllFromUser", "Reading", new { rmessage = ReadingMessageId.DeleteReadingFailed });
            }
        }

        #region Helpers

        public enum ActiveReadingMessageId
        {
            AddActiveReadingSuccess,
            AddActiveReadingFailed,
            ActiveReadingNotFound,
            NotAuthorized,
            DeleteActiveReadingSuccess,
            DeleteActiveReadingFailed,
            ReadingNameAlreadyExists,
            ValidationFailed,
            Error,
            DeviceNotFound
        }

        public enum ReadingMessageId
        {
            ReadingNotFound,
            NotAuthorized,
            DeleteReadingSuccess,
            DeleteReadingFailed,
            ValidationFailed,
            Error,
            DeviceNotFound
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}