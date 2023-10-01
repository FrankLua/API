using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.ResponceModels;

namespace API.Services.ForDB.Int
{
    public interface IDevice_Service
    {
        Task<BaseResponse<DeviceResponce>> GetDevice(string id);

        BaseResponse<DeviceResponce> GetDeviceSync(string id);

        Task<BaseResponse<bool>> EditDevice(Device device);

        Task<BaseResponse<bool>> CreateDevice(Device device, string login);

        Task<BaseResponse<bool>> DeleteDevice(string deviceid, string login);

    }
}
