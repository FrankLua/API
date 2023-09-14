using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.ResponceModels;

namespace API.Services.ForAPI.Int
{
    public interface IUser_Service
    {
        public Task<BaseResponse<User>> CheakUser(string username, string password);


        public Task<BaseResponse<DataResponce>> GetUserDevice(string login);

        public Task<List<string>> GetUserFilesId(string login);

        public Task<BaseResponse<UserResponce>> GetUserInfo(string login);
    }
}
