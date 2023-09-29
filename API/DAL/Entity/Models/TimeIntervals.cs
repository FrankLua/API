using MongoDB.Bson.Serialization.Attributes;

namespace API.DAL.Entity.Models
{
    public struct TimeIntervals
    {
        private static List<string> weekday = new List<string>() {"Понедельник","Вторник","Среда","Четверг","Пятница","Суббота","Воскресенье" };
        [BsonElement("day")]
        public string day { get; set; }

		
		[BsonElement("from")]
        public string from { get; set; }

		
		[BsonElement("to")]
        public string to { get; set; }


        TimeIntervals(string day, string from, string to)
        {
            this.day = day;
            this.from = from;
            this.to = to;
		}
        public static List<TimeIntervals> ParceArray (string[] timeIntervals)
        {
            List<TimeIntervals > result = new List<TimeIntervals>();
            foreach (var timeInterval in timeIntervals)
            {
                var day = timeInterval.Split('/')[0];
                var from = timeInterval.Split('/')[1].Split(" - ")[0];
                var to = timeInterval.Split('/')[1].Split(" - ")[1];
                TimeIntervals newInterval = new TimeIntervals(day, from, to); 
                result.Add(newInterval);
            }
            return result;
        }
        public static List<TimeIntervals> GetNewWeek()
        {
            var list = new List<TimeIntervals>(); 
            foreach(var day in weekday)
            {
                var interval = new TimeIntervals();
                interval.day = day;
                interval.from = "00:00";
				interval.to = "00:00";
				list.Add(interval);
            }
            return list;
        }

    }


}
