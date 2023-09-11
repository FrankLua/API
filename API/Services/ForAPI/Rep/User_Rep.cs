using MongoDB.Driver;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.ResponceModels;
using API.DAL.Entity.SupportClass;
using API.DAL.Entity.APIDatebaseSet;
using API.Services.ForAPI.Int;
using System.Linq;

namespace API.Services.ForAPI.Rep
{
    public class User_Rep : IUser_Service
    {
        private readonly IMongoCollection<Media_file> _media_file;
        private readonly IMongoCollection<User> _user;
        private readonly IMongoCollection<Device> _device;
        public User_Rep(IAPIDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _user = database.GetCollection<User>("Users");
            _device = database.GetCollection<Device>("Device");
            _media_file = database.GetCollection<Media_file>("Media Files");
        }

        public async Task<BaseResponse<User>> CheakUser(string username, string password)
        {
            BaseResponse<User> response = new BaseResponse<User>();

            try
            {
                User user = await _user.Find(user => user.login == username && user.password == password).FirstAsync();

                if (user != null)
                {
                    response.data = user;
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
                Loger.Exaption(ex, "GetUserInfo");
                answer.error = "Crush";
                return answer;
            }
        }

        public async Task<BaseResponse<DataResponce>> GetUserDevice(string login)
        {

            BaseResponse<DataResponce> answer = new BaseResponse<DataResponce>();

            try
            {
                User user = await _user.FindAsync(user => user.login == login).Result.FirstAsync();
                var list = _device.Find(device => true).ToList();


				List<Device> listdevice =  await _device.FindAsync(device => user.devices.Contains(device._id)).Result.ToListAsync(); 
                
				Loger.WriterLogMethod("GetUserDevice", "I read it, users db");
				if (listdevice != null && user.devices != null)
                {
                    answer.data = new DataResponce();
                    List<DeviceResponce> userdevice = new List<DeviceResponce>();
                    foreach (string id in user.devices)
                    {
                        foreach (Device device in listdevice)
                        {
                            if (device._id == id)
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
            catch (Exception ex)
            {
                Loger.Exaption(ex,"Get-User-Device");
                answer.error = "Crush!";
                return answer;
            }
        }

        public async Task<List<string>> GetUserFilesId(string login)
        {
            List<string> list = new List<string>();
            try
            {
                var user = await _user.Find(user => user.login == login).FirstAsync();
                list = user.media_files.ToList();
                return list;
            }
            catch (Exception ex)
            {
                Loger.Exaption(ex, "GetUserFilesId");
                return null;
            }

        }
    }
}
