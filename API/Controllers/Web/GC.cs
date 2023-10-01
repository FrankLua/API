using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Web
{
    public class GC : Controller
    {
        [Route("Web/Gc")]
        [HttpGet]
        [Authorize]
        public IActionResult GCs()
        {
            long totalMemory = System.GC.GetTotalMemory(false);
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();

            return RedirectPermanent("~/Web/Home/Main");
        }
    }
}
