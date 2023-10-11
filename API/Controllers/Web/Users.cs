using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Web
{
    public class Users : Controller
    {
        [Route("Web/User/Create")]
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            long totalMemory = System.GC.GetTotalMemory(false);
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();

            return RedirectPermanent("~/Web/Home/Main");
        }
    }
}
