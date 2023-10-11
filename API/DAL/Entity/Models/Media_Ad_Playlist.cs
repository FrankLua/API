using Amazon.Runtime.SharedInterfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace API.DAL.Entity.Models
{
	public class Ad_playlist
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		virtual public string _id { get; set; }

		[BsonElement("name")]
		public string name { get; set; }

		[BsonElement("ad_files")]

		public List<ad_files>? ad_files { get; set; }


	}
	public class Media_Ad_playlist : Ad_playlist
	{
        
		public Media_Ad_playlist()
		{
			ad_files = new List<ad_files>();
		}

		public void SetFiles(List<string> ids)
		{
			if (ad_files != null)
			{
				foreach (var item in ids)
				{
					var entity = new ad_files() { file = item };
					ad_files.Add(entity);
				}
			}
        }

		public IEnumerable<string> GetIdPlaylists ()
		{
			if(ad_files != null)
			{
				foreach (var item in this.ad_files)
				{
					yield return item.file;
				}
			}			
		}

	}
	public class Media_Ad_playlist_for_API : Ad_playlist
	{
		public string _id { get; set; }
		public Media_Ad_playlist_for_API( Media_Ad_playlist newPlaylist)
		{
			this._id = newPlaylist._id.ToString();
			this.name = newPlaylist.name;			
			this.ad_files = newPlaylist.ad_files;			

		}
	}
	public struct ad_files
	{
		public ad_files(string file, string  start_time, string interval)
		{
			if(interval == "")
			{
                this.file = file;
                this.start_time = start_time;
                this.interval = null;
			}
			else
			{
                this.file = file;
                this.start_time = start_time;
                this.interval = interval;
            }
			
			
		}
		[BsonElement("file")]
		[JsonPropertyName("id")]
		public string file { get; set; }
		[BsonElement("time")]
		public string  start_time { get; set; }

		[BsonElement("interval")]
		public string interval { get; set; }

	}




}
