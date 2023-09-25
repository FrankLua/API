using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.ResponceModels;

namespace API.Services.ForAPI.Int
{
    public interface IDevice_Service
    {
		Task<BaseResponse<DeviceResponce>> GetDevice(string id);

        BaseResponse<DeviceResponce> GetDeviceSync(string id);

        Task<BaseResponse<bool>> EditDevice(Device device);
    }
}
