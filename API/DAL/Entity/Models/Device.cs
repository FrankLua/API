﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace API.DAL.Entity.Models
{


    public class Device
    {
		public Device(string name, string adress)
		{
			_id = ObjectId.GenerateNewId().ToString();
			this.name = name;
			this.adress = adress;
			this.intervals = TimeIntervals.GetNewWeek();

		}
		[BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        [BsonElement("name")]
        public string name { get; set; }    
        [BsonElement("address")]
        public string adress { get; set; }
        [BsonElement("media_playlist")]
        public string? media_play_list { get; set; }


        [BsonElement("ad_playlist")]
        public string? ad_playlist { get; set; }

        [BsonElement("time_intervals")]
        public List<TimeIntervals>? intervals { get; set; }
		

	}

}
