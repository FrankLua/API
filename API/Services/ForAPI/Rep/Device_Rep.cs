using Amazon.Runtime.Internal.Util;
using API.DAL.Entity.APIDatebaseSet;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.ResponceModels;
using API.DAL.Entity.SupportClass;
using API.Services.ForAPI.Int;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace API.Services.ForAPI.Rep
{
    public class Device_Rep : IDevice_Service
    {
        private readonly IMongoCollection<Device> _device;
        private readonly IMongoCollection<User> _user;
        IMemoryCache _cache;
        public Device_Rep(IAPIDatabaseSettings settings, IMongoClient mongoClient, IMemoryCache memoryCache)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _device = database.GetCollection<Device>("Device");
            _user = database.GetCollection<User>("Users");
            _cache = memoryCache;
        }


        public async Task<BaseResponse<DeviceResponce>> GetDevice(string id)
        {

            BaseResponse<DeviceResponce> answer = new BaseResponse<DeviceResponce>();
            try
            {
                _cache.TryGetValue(id, out Device? device);
                if(device== null)
                {
                    var bridgh = await _device.FindAsync(device => device._id == id);
                    device = await bridgh.FirstAsync();                    
                    answer.data = new DeviceResponce(device);
                    _cache.Set(id, device, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
                else
                {
                    answer.data = new DeviceResponce(device);
                }                             
                
                
                return answer;
            }
            catch (InvalidOperationException ex)
            {
                string[] par = new string[] {"Device"};
                Loger.ExaptionForNotFound(ex,method:"GetDevice",id, par);
                answer.error = "Crush";
                return answer;
            }
            catch (Exception ex) 
            {
                Loger.Exaption(ex, "GetDevice");
                answer.error = "Crush";
                return answer;
            }

        }
        public BaseResponse<DeviceResponce> GetDeviceSync(string id)
        {

            BaseResponse<DeviceResponce> answer = new BaseResponse<DeviceResponce>();
            try
            {
                var bridgh = _device.Find(device => device._id == id);
                Device device = bridgh.First();
                if (device == null)
                {
                    answer.error = "Device not found!";
                    return answer;
                }
                answer.data = new DeviceResponce(device);
                return answer;
            }
            catch (Exception ex)
            {
                Loger.Exaption(ex, "GetDevice");
                answer.error = "Crush";
                return answer;
            }

        }



    }
}
