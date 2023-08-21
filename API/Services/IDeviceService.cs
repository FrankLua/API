using API.Entity.APIResponce;
using API.Entity.Models;

namespace API.Services
{
    public interface IDeviceService
    {
        BaseResponse<List<Device>> GetDevicebyId(int id);

        BaseResponse<List<int>> GetDeviceIdbyUserId(int id);


        Device GetbyID(int id);

        Device Create(Device device);

        void Update (int id, Device device);

        void Delete(int id);

    }
}
