using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PoolServer.Entities;

namespace PoolGameServer.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        [BsonIgnore]
        public string Token { get; set; }

        public UserBalance Balance { get; set; }

        public UserExperience Experience { get; set; }
        
        public UserStatistics Statistics { get; set; }
        
        public UserFriends Friends { get; set; }

        public UserLocation Location { get; set; }
        
        public BsonTimestamp TimeStamp { get; set; }
    }
}