using API.DAL.Entity.Models;

namespace API.DAL.Entity.ResponceModels
{
    public class DeviceResponce
    {
        public int id {  get; set; } 

        public string name { get; set; }

        public string address { get; set; }

        public int media_playlist { get; set; }

        public int? ad_playlist { get; set; }

        public List<TimeIntervals>? intervals { get; set; }

        public DeviceResponce(Device device)
        {
            id = device.id;

            name = device.name;

            address = device.adress;

            media_playlist = device.media_play_list;

            ad_playlist = device.ad_playlist;

            intervals = device.intervals;
        }


    }
}
