using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;

namespace API.Services
{
    public interface IDeviceService
    {
        BaseResponse<DataResponce> GetDevicebyId(int id);

        BaseResponse<List<int>> GetDeviceIdbyUserId(int id);


        Device GetbyID(int id);

        Device Create(Device device);

        void Update (int id, Device device);

        void Delete(int id);

    }
}
