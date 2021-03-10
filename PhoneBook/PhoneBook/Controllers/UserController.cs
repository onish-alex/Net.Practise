namespace PhoneBook.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Mvc;
    using PhoneBook.Services;
    using PhoneBook.ViewModels;

    public class UserController : Controller
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Login([FromQuery] string returnUrl = null)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.Redirect(returnUrl ?? "/");
            }

            this.ViewBag.ReturnUrl = returnUrl;

            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            [FromForm]LoginViewModel model,
            [FromQuery]string returnUrl = null)
        {
            if (this.ModelState.IsValid)
            {
                var result = await this.userService.LoginAsync(model);

                if (result)
                {
                    return this.LocalRedirect(returnUrl ?? "/");
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
            if (this.ModelState.IsValid)
            {
                var result = await this.userService.RegisterAsync(model);

                if (result)
                {
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
    }
}
