using API.DAL.Entity.Models;

namespace API.DAL.Entity.APIResponce
{
    public class DataResponce
    {
        public List<Device> devices { get; set; }
        public DataResponce() 
        {
        devices = new List<Device>();
        }

    }


}
