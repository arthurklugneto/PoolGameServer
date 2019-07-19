using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PoolGameServer.Entities
{
    public class UserFriends
    {
        [BsonIgnore]
        public List<User> Friends { get; set; }

        public List<string> FriendsIds { get; set; }
    }
}