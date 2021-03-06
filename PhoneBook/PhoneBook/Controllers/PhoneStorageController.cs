namespace PhoneBook.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PhoneBook.Models;
    using PhoneBook.Services;
    using PhoneBook.ViewModels;

    public class PhoneStorageController : Controller
    {
        private IPhoneService phoneService;

        public PhoneStorageController(IPhoneService phoneService)
        {
            this.phoneService = phoneService;
        }

        [Authorize]
        public IActionResult Index(int pageSize = 20, int pageLinkCount = 20, int page = 1)
        {
            PagingListViewModel<BasePhoneViewModel> model;
            try
            {
                model = this.phoneService.GetPage(page, pageSize, pageLinkCount);
            }
            catch (Exception e)
            {
                return this.RedirectToAction("Error", "PhoneStorage", new { message = e.Message });
            }

            return this.View(model);
        }

        public IActionResult Phone(Guid id)
        {
            var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ConcretePhoneViewModel model;
            try
            {
                model = this.phoneService.GetConcretePhone(id, Guid.Parse(userId));
            }
            catch (Exception e)
            {
                return this.RedirectToAction("Error", "PhoneStorage", new { message = e.Message });
            }

            return this.View(model);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var editPhoneModel = this.phoneService.GetEditModel(id);
            var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userGuid = Guid.Parse(userId);

            if (editPhoneModel.CreatorId != userGuid)
            {
                return this.View("AccessDenied");
            }

            return this.View(editPhoneModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditPhoneViewModel editPhoneViewModel)
        {
            var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userGuid = Guid.Parse(userId);

            if (editPhoneViewModel.CreatorId != userGuid)
            {
                return this.View("AccessDenied");
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    await this.phoneService.EditPhoneAsync(editPhoneViewModel, userGuid);
                }
                catch
                {
                    return this.View("AccessDenied");
                }

                return this.RedirectToAction("Phone", new { id = editPhoneViewModel.Id });
            }

            return this.View(editPhoneViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var createViewModel = new CreatePhoneViewModel();
            createViewModel.StatusNames = this.phoneService.GetStatusNames();
            return this.View(createViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePhoneViewModel createViewModel)
        {
            var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userGuid = Guid.Parse(userId);

            if (this.ModelState.IsValid)
            {
                var createdPhone = await this.phoneService.CreatePhoneAsync(createViewModel, userGuid);

                return this.View("Phone", createdPhone);
            }

            return this.View(createViewModel);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            return this.View(id);
        }

        public async Task<IActionResult> DeleteConfirm(Guid id)
        {
            var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userGuid = Guid.Parse(userId);

            try
            {
                await this.phoneService.DeletePhoneAsync(id, userGuid);
            }
            catch
            {
                return this.View("AccessDenied");
            }

            return this.RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message)
        {
            return this.View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                Message = message ?? string.Empty,
            });
        }
    }
}
