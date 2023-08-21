using API.Entity;
using API.Entity.APIResponce;
using API.Entity.Models;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace API.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IMongoCollection<Device> _device;
        public DeviceService(IAPIDatabaseSettings settings,IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _device = database.GetCollection<Device>(settings.NameCollection);
        }
        public Device Create(Device device)
        {
            _device.InsertOne(device);
            return device;
        }

        public void Delete(int id)
        {
            _device.DeleteOne(device => device.id == id);
        }

        public BaseResponse<List<Device>> GetDevicebyId(int id)
        {
            BaseResponse< List < Device >> answer  = new BaseResponse< List < Device > >();
            try
            {
                answer.data = _device.Find(device => true).ToList();
                if(answer.data.Count == 0) { 

                    answer.error = "Device not found!";
                    return answer;
                }
                answer.data = answer.data.Where(d => d.id == id).ToList();
                answer.error = null;
                return answer;
            }
            catch(Exception ex)
            {
                answer.error = "Crush";
                return answer;
            }
            
        }

        public Device GetbyID(int id)
        {
            return _device.Find(device => device.id == id).FirstOrDefault();

        }

        public void Update(int id, Device device)
        {
            _device.ReplaceOne(device => device.id == id, device);
        }

        public BaseResponse<List<int>> GetDeviceIdbyUserId(int id)
        {
            BaseResponse<List<int>> answer = new BaseResponse<List<int>>();
            answer.data = new List<int>();
            List<Device> list = new List<Device> ();
            try
            {
                list = _device.Find(device => true).ToList();
                list = list.Where(device => device.User_Id == id).ToList();
                foreach(Device device in list)
                {
                    int nowid = device.id;
                    answer.data.Add(nowid);
                }
                if (answer.data.Count == 0)
                {
                    answer.error = "Device not found!";
                    return answer;
                }                
                answer.error = null;
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
