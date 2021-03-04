namespace PhoneBook.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PhoneBook.Data;
    using PhoneBook.Models;
    using PhoneBook.Validation;
    using PhoneBook.ViewModels;

    public class UserController : Controller
    {
        private PhoneBookDbContext dbContext;

        public UserController(
            PhoneBookDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                User user = await this.dbContext.Users.FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
                if (user != null)
                {
                    await this.Authenticate(model.Login);

                    this.HttpContext.Response.Cookies.Append("Id", user.Id.ToString());
                    return this.RedirectToAction("Index", "Home");
                }

                this.ModelState.AddModelError(string.Empty, "Неверно введенный логин и(или) пароль");
            }

            return this.View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (model == null)
            {
                this.ModelState.AddModelError(string.Empty, ValidationMessages.UserRegisterNull);
            }

            if (this.ModelState.IsValid)
            {
                User user = await this.dbContext.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (user == null)
                {
                    this.dbContext.Users.Add(new User { Login = model.Login, Password = model.Password });
                    await this.dbContext.SaveChangesAsync();

                    return this.RedirectToAction("Login", "User");
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Неверно введенный логин и(или) пароль");
                }
            }

            return this.View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return this.RedirectToAction("Login", "User");
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
