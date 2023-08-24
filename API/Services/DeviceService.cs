using API.DAL.Entity;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.ResponceModels;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace API.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IMongoCollection<Device> _device;
        private readonly IMongoCollection<User> _user;
        public DeviceService(IAPIDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _device = database.GetCollection<Device>("Devices");
            _user = database.GetCollection<User>("Users");
        }


        public BaseResponse<DeviceResponce> GetDevice(int id)
        {
            
            BaseResponse<DeviceResponce> answer = new BaseResponse<DeviceResponce> ();            
            try
            {
                Device device  = _device.Find(device => device.id == id).First();
                Loger.WriterLogMethod("GetDevice", "I read it, divice db");
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
                answer.error = "Crush";
                return answer;
            }

        }

        
    }
}
