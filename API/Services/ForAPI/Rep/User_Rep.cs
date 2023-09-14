using MongoDB.Driver;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.ResponceModels;
using API.DAL.Entity.SupportClass;
using API.DAL.Entity.APIDatebaseSet;
using API.Services.ForAPI.Int;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace API.Services.ForAPI.Rep
{
    public class User_Rep : IUser_Service
    {
        private readonly IMongoCollection<Media_file> _media_file;
        private readonly IMongoCollection<User> _user;
        private readonly IMongoCollection<Device> _device;
        private IMemoryCache _cache;
        public User_Rep(IAPIDatabaseSettings settings, IMongoClient mongoClient, IMemoryCache memoryCache)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _user = database.GetCollection<User>("Users");
            _device = database.GetCollection<Device>("Device");
            _media_file = database.GetCollection<Media_file>("Media Files");
            _cache = memoryCache;
        }

        public async Task<BaseResponse<User>> CheakUser(string username, string password)
        {
            BaseResponse<User> response = new BaseResponse<User>();

            try
            {
                
                _cache.TryGetValue(username + password, out User? user);
                if (user == null)
                {
                    var bridge = await _user.FindAsync(user => user.login == username && user.password == password);

                    user = await bridge.FirstAsync();

                    _cache.Set(username += password, user, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                    if (user != null)
                    {
                        response.data = user;
                        return response;
                    }
                    else
                    {
                        response.error = "NotFound";
                        return response;
                    }
                }
                else
                {
                    response.data = user;
                    return response;
                }




            }
            catch (InvalidOperationException ex)
            {
                string[] par = new string[] { "User" };
                Loger.ExaptionForNotFound(ex, method: "CheakUser", username, par);
                response.error = "NotFound";
                return response;
            }
            catch (Exception ex)
            {
                response.error = "Crush";
                return response;
            }

        }

        public async Task<BaseResponse<UserResponce>> GetUserInfo(string login)
        {
            BaseResponse<UserResponce> answer = new BaseResponse<UserResponce>();
            try
            {
                _cache.TryGetValue(login, out User? user);
                if (user == null)
                {
                    var bridgh = await _user.FindAsync(user => user.login == login);
                    user = await bridgh.FirstAsync();
                    answer.data = new UserResponce(user);
                    _cache.Set(login, user, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                    return answer;

                }
                else
                {
                    answer.data = new UserResponce(user);
                    return answer;
                }


            }
            catch (InvalidOperationException ex)
            {
                string[] par = new string[] { "User" };
                Loger.ExaptionForNotFound(ex, method: "GetUserInfo", login, par);
                return null;
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
                _cache.TryGetValue(login, out User? user);
                if (user == null)
                {
                    var bridge_for_user = await _user.FindAsync(user => user.login == login);
                    user = await bridge_for_user.FirstAsync();
                    _cache.Set(login, user, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
                List<Device> list = new List<Device>();
                foreach (string id_device in user.devices)
                {
                    _cache.TryGetValue(id_device, out Device? device);
                    if (device == null)
                    {
                        var brigde = await _device.FindAsync(device => device._id == id_device);
                        device = await brigde.FirstAsync(); 
                        _cache.Set(id_device, device, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                        list.Add(device);
                    }
                    else
                    {
                        list.Add(device);
                    }
                }

                


                if (list != null && user.devices != null)
                {
                    answer.data = new DataResponce();
                    List<DeviceResponce> userdevice = new List<DeviceResponce>();
                    DeviceResponce deviceResponce = new DeviceResponce(list);
                    userdevice.Add(deviceResponce);
                    answer.data.devices = userdevice;

                }

                return answer;
            }
            catch (InvalidOperationException ex)
            {
                string[] par = new string[] { "User or Device" };
                Loger.ExaptionForNotFound(ex, method: "GetUserDevice", login, par);
                answer.error = "Device not found";
                return answer;
            }
            catch (Exception ex)
            {
                Loger.Exaption(ex, "Get-User-Device");
                answer.error = "Crush!";
                return answer;
            }
        }

        public async Task<List<string>> GetUserFilesId(string login)
        {
            List<string> list = new List<string>();
            try
            {
                _cache.TryGetValue(login, out User? user);
                if(user == null)
                {
                    var bridge_for_user = await _user.FindAsync(user => user.login == login);
                    user = await bridge_for_user.FirstAsync();
                    _cache.Set(login, user, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                    list = user.media_files.ToList();
                    return list;
                }
                else
                {
                    list = user.media_files.ToList();
                    return list;
                }
                
            }
            catch (InvalidOperationException ex)
            {
                string[] par = new string[] { "User" };
                Loger.ExaptionForNotFound(ex, method: "GetUserFilesId", login, par);
                return null;
            }
            catch (Exception ex)
            {
                Loger.Exaption(ex, "GetUserFilesId");
                return null;
            }

        }
    }
}
