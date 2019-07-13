using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PoolGameServer.Entities
{
    public class UserBalance
    {
        public long CoinsPool { get; set; }
        public long StarsPool { get; set; }
    }
}