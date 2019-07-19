using PoolGameServer.Entities;

namespace PoolGameServer.Populators
{
    public class UserPopulator : IPopulator<User>
    {
        public User populate(User entity)
        {
            // Populate users experience
            // TODO : find a better formula
            entity.Experience.Level = (entity.Experience.Experience / 1000) + 1;

            // Populate user statistics
            entity.Statistics.MatchesWinsPercentage = entity.Statistics.MatchesWins == 0 ?
                0 : ((decimal)entity.Statistics.MatchesWins/(decimal)entity.Statistics.Matches) * 100;

            entity.Statistics.TournamentsWinsPercentage = entity.Statistics.TournamentsWins == 0?
                0 : ((decimal)entity.Statistics.TournamentsWins/(decimal)entity.Statistics.Tournaments) * 100;

            return entity;
        }
    }
}