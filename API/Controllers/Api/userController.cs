using Amazon.Runtime.Internal;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.ResponceModels;
using API.DAL.Entity.SupportClass;
using API.Entity.SecrurityClass;
using API.Services.ForAPI.Int;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

using System.Text;

namespace API.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class userController : Controller
    {
        private readonly IUser_Service _user;

        public userController(IUser_Service userService)
        {
            _user = userService;
        }
        [HttpGet, BasicAuthorization]
        [Route("")]
        public async Task<BaseResponse<UserResponce>> GetUserInfo()
        {
            await Loger.BeginMethod(Request);
            var userlogin = User.Identity.Name;
            return _user.GetUserInfo(userlogin);
        }

        [HttpGet, BasicAuthorization]
        [Route("devices")]
        public async Task<BaseResponse<DataResponce>> GetUserDevice()
        {
            Loger.BeginMethod(Request);
            var userlogin = User.Identity.Name;
            return await _user.GetUserDevice(userlogin);
        }

    }
}
