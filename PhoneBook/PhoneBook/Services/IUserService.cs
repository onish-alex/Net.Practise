namespace PhoneBook.Services
{
    using System;
    using System.Threading.Tasks;
    using PhoneBook.ViewModels;

    public interface IUserService
    {
        Task<bool> RegisterAsync(RegisterViewModel model);

        Task<bool> LoginAsync(LoginViewModel model);

        Guid GetUserId();
    }
}
