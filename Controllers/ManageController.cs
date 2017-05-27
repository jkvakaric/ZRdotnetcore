using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZRdotnetcore.Models;
using ZRdotnetcore.Models.ManageViewModels;
using ZRdotnetcore.Repos.Interfaces;

namespace ZRdotnetcore.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly IUserRepo _userRepo;

        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          ILoggerFactory loggerFactory,
          IUserRepo userRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<ManageController>();
            _userRepo = userRepo;
        }

        //
        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.ChangeInfoSuccess ? "Your info has been changed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";

            ApplicationUser user = await GetCurrentUserAsync();
            if (user == null) return View("Error");

            var model = new IndexViewModel
            {
                HasPassword = await _userManager.HasPasswordAsync(user),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                Logins = await _userManager.GetLoginsAsync(user),
                BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
                User = _userRepo.Get(user.Id)
            };
            return View(model);
        }

        //
        // GET: /Manage/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            ApplicationUser user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });

            IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                _logger.LogInformation(3, "User changed their password successfully.");
                return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/ChangeInfo
        [HttpGet]
        public IActionResult ChangeInfo()
        {
            var model = new ChangeInfoViewModel
            {
                FullName = _userRepo.Get(GetCurrentUserAsync().Result.Id).FullName
            };
            return View(model);
        }

        //
        // POST: /Manage/ChangeInfo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeInfo(ChangeInfoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            ApplicationUser appuser = await GetCurrentUserAsync();
            if (appuser == null) return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });

            User user = _userRepo.Get(GetCurrentUserAsync().Result.Id);
            user.FullName = model.FullName;

            _userRepo.Update(user);
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangeInfoSuccess });
        }

        //
        // GET: /Manage/GetAccountId
        [HttpGet]
        public IActionResult GetAccountId()
        {
            var model = new GetAccountIdViewModel
            {
                UserId = _userRepo.Get(GetCurrentUserAsync().Result.Id).UserId
            };
            return PartialView(model);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            ChangeInfoSuccess,
            Error
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}
