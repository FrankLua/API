using MongoDB.Driver;
using API.DAL.Entity;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.ResponceModels;

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

        public BaseResponse<UserResponce> GetUserInfo(string login)
        {
            BaseResponse<UserResponce> answer = new BaseResponse<UserResponce>();
            try
            {
                User user = _user.Find(user => true).ToList().First();
                List<Device> list = _device.Find(device => true).ToList();
                List<int> deviceid = new List<int>();
                foreach (Device device in list)
                {
                    if(device.User_Id == user.id)
                    {
                       deviceid.Add(device.id);
                    }

                }
                if(deviceid.Count > 0)
                {
                    user.devices = new int[deviceid.Count];
                    user.devices = deviceid.ToArray();
                }
                answer.data = new UserResponce(user);
                return answer;
                
            }
            catch (Exception ex)
            {
                answer.error = "Crush";
                return answer;
            }
        }

        public BaseResponse<DataResponce> GetUserDevice(string login)
        {
            BaseResponse<DataResponce> answer = new BaseResponse<DataResponce>();           
            answer.data = new DataResponce();
            try
            {
                User user = _user.Find(user => true).ToList().Where(user => user.login == login).First();
                
                answer.data.devices = _device.Find(divice => true).ToList().Where(device => device.User_Id == user.id).ToList();
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
