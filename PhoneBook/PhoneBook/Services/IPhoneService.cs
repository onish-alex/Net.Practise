namespace PhoneBook.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PhoneBook.ViewModels;

    public interface IPhoneService
    {
        PagingListViewModel<BasePhoneViewModel> GetPage(int page, int pageSize, int pageLinkCount);

        ConcretePhoneViewModel GetConcretePhone(Guid phoneId, Guid userId);

        Task EditPhoneAsync(EditPhoneViewModel phoneViewModel, Guid userId);

        EditPhoneViewModel GetEditModel(Guid phoneId);

        IEnumerable<string> GetStatusNames();

        Task<ConcretePhoneViewModel> CreatePhoneAsync(CreatePhoneViewModel phoneViewModel, Guid userId);

        Task DeletePhoneAsync(Guid phoneId, Guid userId);
    }
}
