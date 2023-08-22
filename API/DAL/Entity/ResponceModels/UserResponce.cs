using API.DAL.Entity.Models;

namespace API.DAL.Entity.ResponceModels
{
    public class UserResponce
    {

        public int id { get; set; }

        public string role { get; set; }

        public int[] devices { get; set; }
        public UserResponce(User user) {

            id = user.id;
            role = user.role;
            devices = user.devices;
        }
    }
}
