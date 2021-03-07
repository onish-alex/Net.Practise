namespace PhoneBook.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PhoneBook.Models;
    using PhoneBook.Services;
    using PhoneBook.ViewModels;

    [Authorize]
    public class PhoneStorageController : Controller
    {
        private IPhoneService phoneService;
        private IMapper mapper;

        public PhoneStorageController(
            IPhoneService phoneService,
            IMapper mapper)
        {
            this.phoneService = phoneService;
            this.mapper = mapper;
        }

        public IActionResult Index(int pageSize = 20, int pageLinkCount = 20, int page = 1)
        {
            EntityPage<BookEntry> model;
            try
            {
                model = this.phoneService.GetPage(new PageSelectViewModel()
                {
                    Number = page,
                    LinkCount = pageLinkCount,
                    Size = pageSize,
                });
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
            var userGuid = Guid.Parse(userId);
            BookEntry entity;
            try
            {
                entity = this.phoneService.GetConcretePhone(id);
            }
            catch (Exception e)
            {
                return this.RedirectToAction("Error", "PhoneStorage", new { message = e.Message });
            }

            var viewModel = this.mapper.Map<ConcretePhoneViewModel>(entity);
            viewModel.IsCreator = userGuid == entity.CreatorId;

            return this.View(viewModel);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var phoneEntity = this.phoneService.GetEditModel(id);
            var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userGuid = Guid.Parse(userId);

            if (phoneEntity.CreatorId != userGuid)
            {
                return this.View("AccessDenied");
            }

            var viewModel = this.mapper.Map<EditPhoneViewModel>(phoneEntity);
            viewModel.StatusNames = this.phoneService.GetStatusNames();

            return this.View(viewModel);
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
                var createdPhoneEntity = await this.phoneService.CreatePhoneAsync(createViewModel, userGuid);

                var viewModel = this.mapper.Map<CreatePhoneViewModel>(createdPhoneEntity);
                viewModel.StatusNames = this.phoneService.GetStatusNames();

                return this.View("Phone", viewModel);
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
