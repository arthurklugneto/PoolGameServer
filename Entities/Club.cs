using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PoolGameServer.Entities
{
    public class Club
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        [BsonIgnore]
        public List<User> Members { get; set; }

        public List<string> MemberIds { get; set; }
        
        public string Leader { get; set; }
    }
}