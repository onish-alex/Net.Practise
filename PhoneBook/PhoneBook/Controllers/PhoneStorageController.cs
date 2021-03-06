﻿namespace PhoneBook.Controllers
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
        private IUserService userService;
        private IMapper mapper;

        public PhoneStorageController(
            IPhoneService phoneService,
            IUserService userService,
            IMapper mapper)
        {
            this.phoneService = phoneService;
            this.userService = userService; 
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
            var userId = this.userService.GetUserId();
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
            viewModel.IsCreator = userId == entity.CreatorId;

            return this.View(viewModel);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var phoneEntity = this.phoneService.GetEditModel(id);
            var userId = this.userService.GetUserId();

            if (phoneEntity.CreatorId != userId)
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
            var userId = this.userService.GetUserId();

            if (editPhoneViewModel.CreatorId != userId)
            {
                return this.View("AccessDenied");
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    await this.phoneService.EditPhoneAsync(editPhoneViewModel, userId);
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
            var userId = this.userService.GetUserId();

            if (this.ModelState.IsValid)
            {
                var createdPhoneEntity = await this.phoneService.CreatePhoneAsync(createViewModel, userId);

                var viewModel = this.mapper.Map<ConcretePhoneViewModel>(createdPhoneEntity);
                viewModel.IsCreator = true;

                return this.View("Phone", viewModel);
            }

            createViewModel.StatusNames = this.phoneService.GetStatusNames();
            return this.View(createViewModel);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            return this.View(id);
        }

        public async Task<IActionResult> DeleteConfirm(Guid id)
        {
            var userId = this.userService.GetUserId();

            try
            {
                await this.phoneService.DeletePhoneAsync(id, userId);
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
