using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.ResponceModels;

namespace API.Services
{
    public interface IUserService
    {
        public BaseResponse<User>  CheakUser(string username, string password);


        public BaseResponse<DataResponce> GetUserDevice(string login);

        public BaseResponse<UserResponce> GetUserInfo(string login);
    }
}
