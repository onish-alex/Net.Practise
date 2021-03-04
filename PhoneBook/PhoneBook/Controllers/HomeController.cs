namespace PhoneBook.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using PhoneBook.Data;
    using PhoneBook.Models;
    using PhoneBook.ViewModels;

    public class HomeController : Controller
    {
        private PhoneBookDbContext dbContext;
        private Mapper mapper;
        private IConfiguration configuration;

        public HomeController(
            PhoneBookDbContext dbContext,
            Mapper mapper,
            IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [Authorize]
        public IActionResult Index(int page = 1)
        {
            if (page <= 0)
            {
                page = 1;
            }

            var pageSize = this.configuration.GetValue<int>("ApplicationData:PageSize:Default");
            var pageLinkCount = this.configuration.GetValue<int>("ApplicationData:PageLinkCount:Default");

            var phones = this.dbContext.Entries
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var allPageCount = (int)Math.Ceiling(this.dbContext.Entries.Count() / (double)pageSize);

            var phoneViewModels = this.mapper.Map<IEnumerable<PhoneViewModel>>(phones).ToList();

            var minPage = (page - (pageLinkCount / 2) < 1)
                        ? 1
                        : page - (pageLinkCount / 2);

            var maxPage = (minPage + pageLinkCount - 1 < allPageCount)
                        ? minPage + pageLinkCount - 1
                        : allPageCount;

            var model = new PhoneListViewModel()
            {
                Page = page,
                Phones = phoneViewModels,
                MinPage = minPage,
                MaxPage = maxPage,
            };

            return this.View(model);
        }

        public IActionResult Phone(Guid id)
        {
            var phone = this.dbContext.Entries.Find(id);
            var phoneViewModel = this.mapper.Map<PhoneViewModel>(phone);

            var hasCookie = this.HttpContext.Request.Cookies.TryGetValue("Id", out string userId);

            if (!hasCookie)
            {
                return this.View("Error");
            }

            var concretePhone = new ConcretePhoneViewModel()
            {
                Phone = phoneViewModel,
                IsCreator = Guid.Parse(userId) == phoneViewModel.CreatorId,
            };

            return this.View(concretePhone);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var phone = this.dbContext.Entries.Find(id);
            var phoneViewModel = this.mapper.Map<PhoneViewModel>(phone);

            var hasCookie = this.HttpContext.Request.Cookies.TryGetValue("Id", out string userId);

            if (!hasCookie)
            {
                return this.View("Error");
            }

            if (phoneViewModel.CreatorId != Guid.Parse(userId))
            {
                return this.View("AccessDenied");
            }

            return this.View(phoneViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PhoneViewModel phoneViewModel)
        {
            var hasCookie = this.HttpContext.Request.Cookies.TryGetValue("Id", out string userId);

            if (!hasCookie)
            {
                return this.View("Error");
            }

            if (phoneViewModel.CreatorId != Guid.Parse(userId))
            {
                return this.View("AccessDenied");
            }

            if (this.ModelState.IsValid)
            {
                var phone = await this.dbContext.Entries.FindAsync(phoneViewModel.Id);
                phone.LastUpdateDate = phoneViewModel.LastUpdateDate;
                phone.PhoneNumber = phoneViewModel.PhoneNumber;
                phone.Address = phoneViewModel.Address;
                phone.LastUpdateDate = DateTime.Now;
                phone.Status = phoneViewModel.Status;

                this.dbContext.Entries.Update(phone);
                await this.dbContext.SaveChangesAsync();

                return this.View("Phone", new ConcretePhoneViewModel()
                {
                    Phone = phoneViewModel,
                    IsCreator = true,
                });
            }

            return this.View(phoneViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhoneViewModel phoneViewModel)
        {
            var hasCookie = this.HttpContext.Request.Cookies.TryGetValue("Id", out string userId);

            if (!hasCookie)
            {
                return this.View("Error");
            }

            if (this.ModelState.IsValid)
            {
                phoneViewModel.CreatorId = Guid.Parse(userId);
                phoneViewModel.CreationDate = DateTime.Now;
                phoneViewModel.LastUpdateDate = DateTime.Now;
                var phone = this.mapper.Map<BookEntry>(phoneViewModel);

                await this.dbContext.Entries.AddAsync(phone);
                await this.dbContext.SaveChangesAsync();
                phoneViewModel.Id = phone.Id;

                return this.View("Phone", new ConcretePhoneViewModel()
                {
                    Phone = phoneViewModel,
                    IsCreator = true,
                });
            }

            return this.View(phoneViewModel);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            return this.View(id);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteConfirm(Guid id)
        {
            var phoneToDelete = this.dbContext.Entries.Find(id);
            this.dbContext.Entries.Remove(phoneToDelete);
            await this.dbContext.SaveChangesAsync();

            return this.RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
