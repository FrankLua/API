using MongoDB.Driver;
using API.DAL.Entity;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.ResponceModels;

namespace API.Services.ForAPI
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _user;
        private readonly IMongoCollection<Device> _device;
        public UserService(IAPIDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _user = database.GetCollection<User>("Users");
            _device = database.GetCollection<Device>("Devices");
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
                User user = _user.Find(user => true).ToList().Where(user => user.login == login).First();
                Loger.WriterLogMethod("GetUserInfo", "I read it, users db");
                List<Device> list = _device.Find(device => true).ToList();
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

            try
            {
                User user = _user.Find(user => true).ToList().Where(user => user.login == login).First();
                Loger.WriterLogMethod("GetUserDevice", "I read it, users db");
                List<Device> listdevice = _device.Find(divice => true).ToList();
                if (listdevice != null && user.devices != null)
                {
                    answer.data = new DataResponce();
                    List<DeviceResponce> userdevice = new List<DeviceResponce>();
                    foreach (int id in user.devices)
                    {
                        foreach (Device device in listdevice)
                        {
                            if (device.id == id)
                            {
                                DeviceResponce deviceResponce = new DeviceResponce(device);
                                userdevice.Add(deviceResponce);

                            }
                        }
                    }
                    answer.data.devices = userdevice;

                }

                if (answer.data == null)
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
