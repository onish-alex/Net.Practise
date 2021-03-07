namespace PhoneBook.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using PhoneBook.Data;
    using PhoneBook.Models;
    using PhoneBook.ViewModels;

    public class PhoneService : IPhoneService
    {
        private PhoneBookDbContext dbContext;
        private IMapper mapper;

        public PhoneService(
            PhoneBookDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public EntityPage<BookEntry> GetPage(PageSelectViewModel pageSelectViewModel)
        {
            if (pageSelectViewModel.Size < 1)
            {
                throw new ArgumentException("Некорректный размер страницы вывода");
            }

            var entriesCount = this.dbContext.Entries.Count();

            if (entriesCount == 0)
            {
                return new EntityPage<BookEntry>();
            }

            var allPageCount = (int)Math.Ceiling(entriesCount / (double)pageSelectViewModel.Size);

            if (pageSelectViewModel.Number < 1
             || pageSelectViewModel.Number > allPageCount)
            {
                throw new ArgumentException("Некорректный номер страницы вывода");
            }

            var phones = this.dbContext.Entries
                .Skip((pageSelectViewModel.Number - 1) * pageSelectViewModel.Size)
                .Take(pageSelectViewModel.Size)
                .ToList();

            var minPage = (pageSelectViewModel.Number - (pageSelectViewModel.LinkCount / 2) < 1)
                        ? 1
                        : pageSelectViewModel.Number - (pageSelectViewModel.LinkCount / 2);

            var maxPage = (minPage + pageSelectViewModel.LinkCount - 1 < allPageCount)
                        ? minPage + pageSelectViewModel.LinkCount - 1
                        : allPageCount;

            var result = new EntityPage<BookEntry>(
                phones,
                pageSelectViewModel.Number,
                minPage,
                maxPage,
                allPageCount,
                pageSelectViewModel.Size);

            return result;
        }

        public BookEntry GetConcretePhone(Guid phoneId)
        {
            var phone = this.dbContext.Entries
                .Include(x => x.Status)
                .SingleOrDefault(x => x.Id == phoneId);

            if (phone == null)
            {
                throw new NullReferenceException("Запрашиваемая запись не найдена");
            }

            return phone;
        }

        public async Task EditPhoneAsync(EditPhoneViewModel phoneViewModel, Guid userId)
        {
            var phone = this.dbContext.Entries.Find(phoneViewModel.Id);

            if (phone.CreatorId != userId)
            {
                throw new ArgumentException();
            }

            phone.PhoneNumber = phoneViewModel.PhoneNumber;
            phone.Address = phoneViewModel.Address;
            phone.LastUpdateDate = DateTime.Now;
            phone.Status = await this.dbContext.Statuses.SingleOrDefaultAsync(x => x.Name == phoneViewModel.StatusName);

            this.dbContext.Entries.Update(phone);
            await this.dbContext.SaveChangesAsync();
        }

        public BookEntry GetEditModel(Guid phoneId)
        {
            var phone = this.dbContext.Entries.Find(phoneId);

            if (phone == null)
            {
                throw new ArgumentException("Запрашиваемая запись не найдена!");
            }

            return phone;
        }

        public IEnumerable<string> GetStatusNames()
        {
            return this.dbContext.Statuses.Select(x => x.Name) ?? Enumerable.Empty<string>();
        }

        public async Task<BookEntry> CreatePhoneAsync(CreatePhoneViewModel phoneViewModel, Guid userId)
        {
            var phoneToAdd = this.mapper.Map<BookEntry>(phoneViewModel);

            phoneToAdd.CreatorId = userId;
            phoneToAdd.CreationDate = DateTime.Now;
            phoneToAdd.LastUpdateDate = DateTime.Now;
            phoneToAdd.Status = await this.dbContext.Statuses.SingleOrDefaultAsync(x => x.Name == phoneViewModel.StatusName);

            await this.dbContext.Entries.AddAsync(phoneToAdd);
            await this.dbContext.SaveChangesAsync();

            return phoneToAdd;
        }

        public async Task DeletePhoneAsync(Guid phoneId, Guid userId)
        {
            var phoneToDelete = this.dbContext.Entries.Find(phoneId);

            if (phoneToDelete.CreatorId != userId)
            {
                throw new Exception();
            }

            this.dbContext.Entries.Remove(phoneToDelete);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
