namespace PhoneBook.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PhoneBook.Models;
    using PhoneBook.ViewModels;

    public interface IPhoneService
    {
        EntityPage<BookEntry> GetPage(PageSelectViewModel pageSelectViewModel);

        BookEntry GetConcretePhone(Guid phoneId);

        Task EditPhoneAsync(EditPhoneViewModel phoneViewModel, Guid userId);

        BookEntry GetEditModel(Guid phoneId);

        IEnumerable<string> GetStatusNames();

        Task<BookEntry> CreatePhoneAsync(CreatePhoneViewModel phoneViewModel, Guid userId);

        Task DeletePhoneAsync(Guid phoneId, Guid userId);
    }
}
