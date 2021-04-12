namespace PhoneBook.Services
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using PhoneBook.Data;
    using PhoneBook.Models;
    using PhoneBook.ViewModels;

    public class UserService : IUserService
    {
        private PhoneBookDbContext dbContext;
        private IHttpContextAccessor httpContextAccessor;

        public UserService(
            PhoneBookDbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> LoginAsync(LoginViewModel model)
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);

            if (user == null)
            {
                return false;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await this.httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return true;
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

        public Guid GetUserId()
        {
            var claim = this.httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null || !Guid.TryParse(claim.Value, out var id))
            {
                return Guid.Empty;
            }

            return id;
        }
    }
}
