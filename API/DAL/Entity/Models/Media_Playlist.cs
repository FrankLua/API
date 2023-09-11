using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace API.DAL.Entity.Models
{
    public class Media_playlist
    {
        [BsonId]
        
		public ObjectId _id { get; set; }

		[BsonElement("name")]
		public string name { get; set; }
		[BsonElement("is_public")]
		public bool is_public { get; set; }
        [BsonElement("media_files_id")]

        public List<string> media_files_id {get; set;}

        public Media_playlist()
        {
            media_files_id = new List<string>();
        }

    }
	public class Media_playlist_for_api
	{
		
		public string _id { get; set; }

		
		public string name { get; set; }
		
		public bool is_public { get; set; }
		

		public List<string> media_files_id { get; set; }

		public Media_playlist_for_api(Media_playlist playlist)
		{
			_id = playlist._id.ToString();
			name = playlist.name;
			is_public = playlist.is_public;
			media_files_id = playlist.media_files_id;

		}

	}
}
