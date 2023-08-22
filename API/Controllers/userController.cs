using API.Entity.APIResponce;
using API.Entity.Models;
using API.Entity.SecrurityClass;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class userController : Controller
    {
        private readonly IUserService _user;

        public userController(IUserService userService)
        {
            _user = userService;
        }



        [HttpGet, BasicAuthorization]
        [Route("devices")]
        public BaseResponse<List<Device>> GetUser()
        {
            var userlogin = User.Identity.Name;         

            return _user.GetUserDevice(userlogin); 
        }
    }
}
