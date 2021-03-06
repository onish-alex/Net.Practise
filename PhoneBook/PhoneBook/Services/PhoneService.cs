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

        public PagingListViewModel<BasePhoneViewModel> GetPage(int page, int pageSize, int pageLinkCount)
        {
            if (pageSize < 1)
            {
                throw new ArgumentException("Некорректный размер страницы вывода");
            }

            var entriesCount = this.dbContext.Entries.Count();

            if (entriesCount == 0)
            {
                return new PagingListViewModel<BasePhoneViewModel>();
            }

            var allPageCount = (int)Math.Ceiling(entriesCount / (double)pageSize);

            if (page < 1 || page > allPageCount)
            {
                throw new ArgumentException("Некорректный номер страницы вывода");
            }

            var phones = this.dbContext.Entries
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var phoneViewModels = this.mapper.Map<IList<BasePhoneViewModel>>(phones);

            var minPage = (page - (pageLinkCount / 2) < 1)
                        ? 1
                        : page - (pageLinkCount / 2);

            var maxPage = (minPage + pageLinkCount - 1 < allPageCount)
                        ? minPage + pageLinkCount - 1
                        : allPageCount;

            var result = new PagingListViewModel<BasePhoneViewModel>(
                phoneViewModels,
                page,
                minPage,
                maxPage,
                allPageCount,
                pageSize);

            return result;
        }

        public ConcretePhoneViewModel GetConcretePhone(Guid phoneId, Guid userId)
        {
            var phone = this.dbContext.Entries
                .Include(x => x.Status)
                .SingleOrDefault(x => x.Id == phoneId);

            if (phone == null)
            {
                throw new NullReferenceException("Запрашиваемая запись не найдена");
            }

            var viewModel = this.mapper.Map<ConcretePhoneViewModel>(phone);
            viewModel.IsCreator = userId == phone.CreatorId;

            return viewModel;
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

        public EditPhoneViewModel GetEditModel(Guid phoneId)
        {
            var phone = this.dbContext.Entries.Find(phoneId);
            var statusNames = this.dbContext.Statuses.Select(x => x.Name);

            var editViewModel = this.mapper.Map<EditPhoneViewModel>(phone);
            editViewModel.StatusNames = statusNames;
            return editViewModel;
        }

        public IEnumerable<string> GetStatusNames()
        {
            return this.dbContext.Statuses.Select(x => x.Name) ?? Enumerable.Empty<string>();
        }

        public async Task<ConcretePhoneViewModel> CreatePhoneAsync(CreatePhoneViewModel phoneViewModel, Guid userId)
        {
            var phoneToAdd = this.mapper.Map<BookEntry>(phoneViewModel);

            phoneToAdd.CreatorId = userId;
            phoneToAdd.CreationDate = DateTime.Now;
            phoneToAdd.LastUpdateDate = DateTime.Now;
            phoneToAdd.Status = await this.dbContext.Statuses.SingleOrDefaultAsync(x => x.Name == phoneViewModel.StatusName);

            await this.dbContext.Entries.AddAsync(phoneToAdd);
            await this.dbContext.SaveChangesAsync();

            var createdPhone = this.mapper.Map<ConcretePhoneViewModel>(phoneToAdd);
            createdPhone.IsCreator = true;
            return createdPhone;
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
