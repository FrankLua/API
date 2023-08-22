using API.Entity.Models;
using API.Entity;
using MongoDB.Driver;
using API.Entity.APIResponce;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _user;
        private readonly IMongoCollection<Device> _device;
        public UserService(IAPIDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _user = database.GetCollection<User>("Users");
            _device = database.GetCollection<Device>("Device");
        }
        public BaseResponse<User> CheakUser(string username, string password)
        {
            BaseResponse<User> response = new BaseResponse<User>();
            List<User> users = new List<User>();
            try
            {
                users = _user.Find(user => true).ToList();
                users = users.Where(user => user.login == username && user.password == password).ToList();
                if (users.Count > 0)
                {
                    response.data = users.First();
                    return response;
                }
                response.error = "User not found";
                return response;

            }
            catch (Exception ex)
            {
                response.error = "Crush";
                return response;
            }
            
        }

        public BaseResponse<List<Device>> GetUserDevice(string login)
        {
            BaseResponse<List<Device>> answer = new BaseResponse<List<Device>>();

            try
            {
                User user = _user.Find(user => true).ToList().Where(user => user.login == login).First();
                answer.data = _device.Find(divice => true).ToList().Where(device => device.User_Id == user.id).ToList();
                if(answer.data == null)
                {
                    answer.error = "Device not found";
                    return answer;
                }
 
                return answer;
            }
            catch 
            {
                answer.error = "Crush!";
                return answer;
            }
        }
    }
}
