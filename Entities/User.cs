using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PoolGameServer.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public UserBalance Balance { get; set; }
        
        public BsonTimestamp TimeStamp { get; set; }
    }
}