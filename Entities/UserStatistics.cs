using MongoDB.Bson.Serialization.Attributes;

namespace PoolGameServer.Entities
{
    public class UserStatistics
    {
        public int Matches { get; set; }
        
        public int MatchesWins { get; set; }
        
        public int Tournaments { get; set; }
        
        public int TournamentsWins { get; set; }

        public long PocketedBalls { get; set; }
        
        [BsonIgnore]
        public decimal MatchesWinsPercentage { get; set; }

        [BsonIgnore]
        public decimal TournamentsWinsPercentage { get; set; }
    }
}