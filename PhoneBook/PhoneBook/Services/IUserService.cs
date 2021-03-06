namespace PhoneBook.Services
{
    using System.Threading.Tasks;
    using PhoneBook.ViewModels;

    public interface IUserService
    {
        Task<bool> RegisterAsync(RegisterViewModel model);

        Task<LoginCheckViewModel> LoginAsync(LoginViewModel model);
    }
}
