using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PoolServer.Entities
{
    public class UserExperience
    {
        [BsonIgnore]
        public int Level { get; set; }
        public int Experience { get; set; }
    }
}