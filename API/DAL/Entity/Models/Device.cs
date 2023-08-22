using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace API.DAL.Entity.Models
{


    public class Device
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        [BsonElement("id")]
        public int id { get; set; }        
        [BsonElement("address")]
        public string adress { get; set; }
        [BsonElement("media_playlist")]
        public int media_play_list { get; set; }

        [BsonElement("user_id")]
        public int User_Id { get; set; }
        [BsonElement("ad_playlist")]
        public int? ad_playlist { get; set; }

        [BsonElement("time_intervals")]
        public List<TimeIntervals>? intervals { get; set; }


    }
}
