using API.DAL.Entity.Models;
using API.DAL.Entity.SupportClass;
using API.DAL.Entity.WebEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers.Web
{
    

    public class HomeController : Controller
    {
        [Route("Web/Home/Main")]
        [HttpGet]
        [Authorize]
        public IActionResult Main()
        {
            ViewBag.Role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            return View();
        }



	}
}
