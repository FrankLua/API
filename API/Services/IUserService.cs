using API.Entity.APIResponce;
using API.Entity.Models;

namespace API.Services
{
    public interface IUserService
    {
        public BaseResponse<User>  CheakUser(string username, string password);
    }
}
