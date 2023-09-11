using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace API.DAL.Entity.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        [BsonElement("role")]
        public string role { get; set; }

        [BsonElement("password")]
        public string password { get; set; }
        [BsonElement("login")]
        public string login { get; set; }

        [BsonElement("devices")]
        public string [] devices { get; set; }
		[BsonElement("media-files")]
		public string[] media_files { get; set; }
		[BsonElement("media-playlist")]
		public string[] media_playlists { get; set; }


	}
}
