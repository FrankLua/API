using API.DAL.Entity.Models;
using API.DAL.Entity.WebEntity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using API.Services.ForAPI.Int;

namespace API.Controllers.Web
{
    [Route("Web/Log")]
    public class LogController : Controller
    {
        private readonly IUser_Service _user;
        
        public LogController(IUser_Service userService)
        {
            _user = userService;
        }
        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LogModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await _user.CheakUser(model.login, model.password);
                if (user.data != null)
                {
                    await Authenticate(model.login); // аутентификация

                    return RedirectToAction("Main", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
