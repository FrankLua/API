using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.ResponceModels;
using API.Entity.SecrurityClass;
using API.Services.ForAPI;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

using System.Text;

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
        [Route("")]
        public BaseResponse<UserResponce> GetUserInfo()
        {
            

            var userlogin = User.Identity.Name;

            Loger.WriterLogMethod("GetUserInfo", "Called");

            return  _user.GetUserInfo(userlogin);
        }

        [HttpGet, BasicAuthorization]
        [Route("devices")]
        public BaseResponse<DataResponce> GetUserDevice()
        {
            var userlogin = User.Identity.Name;         

            return _user.GetUserDevice(userlogin); 
        }

    }
}
