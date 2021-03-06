namespace PhoneBook.Services
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using PhoneBook.Data;
    using PhoneBook.Models;
    using PhoneBook.ViewModels;

    public class UserService : IUserService
    {
        private PhoneBookDbContext dbContext;

        public UserService(
            PhoneBookDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<LoginCheckViewModel> LoginAsync(LoginViewModel model)
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);

            var viewModel = new LoginCheckViewModel();
            viewModel.CheckResult = user != null;

            if (viewModel.CheckResult)
            {
                viewModel.UserId = user.Id;
            }

            return viewModel;
        }

        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync(u => u.Login == model.Login);

            if (user == null)
            {
                this.dbContext.Users.Add(new User { Login = model.Login, Password = model.Password });
                await this.dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
