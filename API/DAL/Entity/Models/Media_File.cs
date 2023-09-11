using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Xml.Linq;

namespace API.DAL.Entity.Models
{
	public class ApiFile : IEquatable<ApiFile>
    {
		[BsonId]
		public ObjectId _id { get; set; }

		[BsonElement("name")]
		public string name { get; set; }

		[BsonIgnore]
		public string folder { get; set; }

		[BsonElement("mime_type")]
		public string mime_type { get; set; }
        public bool Equals(ApiFile other)
        {
            if (other is null)
                return false;

            return this._id == other._id;
        }
        public override bool Equals(object obj) => Equals(obj as ApiFile);
        public override int GetHashCode() => (_id).GetHashCode();


    }
	public class Media_file:ApiFile
    {

        [BsonElement("is_public")]
        public bool is_public { get; set; }

    }
	public class Adfile : ApiFile
	{

	}

}
