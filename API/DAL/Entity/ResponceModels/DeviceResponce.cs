using API.DAL.Entity.Models;

namespace API.DAL.Entity.ResponceModels
{
    public class DeviceResponce
    {
        public string id {  get; set; } 

        public string name { get; set; }

        public string address { get; set; }

        public string media_playlist { get; set; }

        public string? ad_playlist { get; set; }

        public List<TimeIntervals>? intervals { get; set; }

        public DeviceResponce(Device device)
        {
            id = device._id;

            name = device.name;

            address = device.adress;

            media_playlist = device.media_play_list;

            ad_playlist = device.ad_playlist;

            intervals = device.intervals;
        }
        public DeviceResponce(List<Device> devices)
        {
            foreach (var device in devices)
            {
                id = device._id;

                name = device.name;

                address = device.adress;

                media_playlist = device.media_play_list;

                ad_playlist = device.ad_playlist;

                intervals = device.intervals;
            }
            
        }


    }
}
