using MongoDB.Bson.Serialization.Attributes;

namespace API.DAL.Entity.Models
{
    public struct TimeIntervals
    {
        [BsonElement("day")]
        public string day { get; set; }
        [BsonElement("from")]
        public DateTime from { get; set; }
        [BsonElement("to")]

        public DateTime to { get; set; }




    }


}
