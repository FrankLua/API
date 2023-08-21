using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace API.Entity.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        [BsonElement("id")]
        public int id { get ; set; }
        [BsonElement("role")]
        public string role { get; set; }

        [BsonElement("password")]
        public string password { get; set; }
        [BsonElement("login")]
        public string login { get; set; }

        [BsonElement("devices")]
        public int[]devices { get; set; }
        
       
    }
}
