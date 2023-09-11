using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace API.DAL.Entity.Models
{
	
	public class Media_Ad_playlist
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string _id { get; set; }

		[BsonElement("name")]
		public string name { get; set; }

		[BsonElement("ad_files")]		

		public List<ad_files>? ad_files { get; set; }		

		
	}
	public struct ad_files
	{
		[BsonElement("file")]
		[JsonPropertyName("id")]
		public string file { get; set; }
		[BsonElement("time")]
		public string start_time { get; set; }

	}




}
