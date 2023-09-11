using API.DAL.Entity.APIDatebaseSet;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.ResponceModels;
using API.DAL.Entity.SupportClass;
using API.Services.ForAPI.Int;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace API.Services.ForAPI.Rep
{
    public class Device_Rep : IDevice_Service
    {
        private readonly IMongoCollection<Device> _device;
        private readonly IMongoCollection<User> _user;
        public Device_Rep(IAPIDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _device = database.GetCollection<Device>("Device");
            _user = database.GetCollection<User>("Users");
        }


        public async Task<BaseResponse<DeviceResponce>> GetDevice(string id)
        {

            BaseResponse<DeviceResponce> answer = new BaseResponse<DeviceResponce>();
            try
            {
                Device device = await _device.FindAsync(device => device._id == id).Result.FirstAsync();
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
                Loger.Exaption(ex,"GetDevice");
                answer.error = "Crush";
                return answer;
            }

        }



	}
}
