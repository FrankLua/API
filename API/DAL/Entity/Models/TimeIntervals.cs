using MongoDB.Bson.Serialization.Attributes;

namespace API.DAL.Entity.Models
{
    public struct TimeIntervals
    {
        [BsonElement("day")]
        public string day { get; set; }

		[BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
		[BsonElement("from")]
        public DateTime from { get; set; }

		[BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
		[BsonElement("to")]
        public DateTime to { get; set; }


        TimeIntervals(string day, string from, string to)
        {
            this.day = day;
            this.from = new DateTime(1969, 4, 22, int.Parse(from), 0, 0);
			this.to = new DateTime(1969, 4, 22, int.Parse(to), 0, 0);
		}
        public static List<TimeIntervals> ParceArray (string[] timeIntervals)
        {
            List<TimeIntervals > result = new List<TimeIntervals>();
            foreach (var timeInterval in timeIntervals)
            {
                var day = timeInterval.Split('/')[2];
                var from = timeInterval.Split("/")[0];
                var to = timeInterval.Split("/")[1];
                TimeIntervals newInterval = new TimeIntervals(day, from, to); 
                result.Add(newInterval);
            }
            return result;
        }


    }


}
