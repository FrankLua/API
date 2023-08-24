using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.ResponceModels;

namespace API.Services
{
    public interface IDeviceService
    {
        BaseResponse<DeviceResponce> GetDevice(int id);
    }
}
