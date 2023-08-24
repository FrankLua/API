using API.DAL.Entity.Models;
using API.DAL.Entity.ResponceModels;

namespace API.DAL.Entity.APIResponce
{
    public class DataResponce
    {
        public List<DeviceResponce> devices { get; set; }

        public DataResponce()
        {
            devices = null;
        }
    }


}
