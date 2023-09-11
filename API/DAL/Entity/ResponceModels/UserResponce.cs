using API.DAL.Entity.Models;

namespace API.DAL.Entity.ResponceModels
{
    public class UserResponce
    {

        public string id { get; set; }

        public string role { get; set; }

        public string[] devices { get; set; }
        public UserResponce(User user) {

            id = user._id;
            role = user.role;
            devices = user.devices;
        }
    }
}
