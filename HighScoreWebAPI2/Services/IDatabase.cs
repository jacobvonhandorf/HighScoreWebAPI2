using Google.Cloud.Datastore.V1;
using HighScoreWebAPI2.Models;
using System.Collections.Generic;

namespace HighScoreWebAPI2.Services
{
    public interface IDatabaseService
    {
        void SaveScore(Score score);
        Score? GetScoreForPlayer(string playerId, string game, string level);
        int CountBetterScores(float playerScore, string game, string level, int limit);
        IEnumerable<Score?> GetScores(string game, string level, int numResults, int skip, bool sortAscending);
        IEnumerable<Score> GetAllPlayerScores(string game, string playerId);
        void SaveScores(IEnumerable<Score> scores);
    }

    public class MockDatabase : IDatabaseService
    {
        private readonly List<Score> scores = new();

        public int CountBetterScores(float playerScore)
        {
            return scores.Count(s => s.Value < playerScore);
        }

        public int CountBetterScores(float playerScore, string game, string level, int limit)
        {
            return Math.Clamp(scores.Count(s => s.Value < playerScore
                && s.Game == game
                && s.Level == level), 0, limit);
        }

        public IEnumerable<Score> GetAllPlayerScores(string game, string playerId)
        {
            return scores.Where(s => s.Game == game
                && s.PlayerId == playerId);
        }

        public Score? GetScoreForPlayer(string playerId, string game, string level)
        {
            return scores.Where(s => s.PlayerId == playerId
                && s.Game == game
                && s.Level == level).FirstOrDefault();
        }

        public IEnumerable<Score> GetScores(string game, string level, int numResults, int skip, bool sortAscending)
        {
            IEnumerable<Score> results = scores.Where(s => s.Game == game
                            && s.Level == level);
            if (sortAscending)
                results = results.OrderBy(s => s.Value);
            else
                results = results.OrderByDescending(s => s.Value);
            return results.Skip(skip).Take(numResults);
        }

        public void SaveScore(Score score)
        {
            scores.Add(score);
        }

        public void SaveScores(IEnumerable<Score> scores)
        {
            foreach (Score score in scores)
            {
                SaveScore(score);
            }
        }
    }
}
