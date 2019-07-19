using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PoolServer.Entities;

namespace PoolGameServer.Entities
{
    public class Country
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int Code { get; set; }

        public string Name { get; set; }

        public string FlagImage { get; set; }
    }
}