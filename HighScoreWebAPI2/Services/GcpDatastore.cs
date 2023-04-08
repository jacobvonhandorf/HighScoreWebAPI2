using Google.Api.Gax;
using Google.Cloud.Datastore.V1;
using Google.Protobuf.Collections;
using HighScoreWebAPI2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace HighScoreWebAPI2.Services
{
    public class GcpDatastore : IDatabaseService
    {
        private DatastoreDb Store { get; set; }

        public GcpDatastore(string projectId)
        {
            Store = DatastoreDb.Create(projectId);
            Console.WriteLine($"Datastore Connection to {projectId} Initialized");
        }

        public GcpDatastore(string projectId, string dbNamespace)
        {
            Store = DatastoreDb.Create(projectId, dbNamespace);
        }

        public Score? GetScoreForPlayer(string playerId, string game, string level)
        {
            Query query = new(nameof(Score))
            {
                Filter = Filter.And(
                    Filter.Equal(nameof(Score.Game), game),
                    Filter.Equal(nameof(Score.Level), level),
                    Filter.Equal(nameof(Score.PlayerId), playerId)),
            };
            Entity? queryResult = Store.RunQuery(query).Entities.FirstOrDefault();
            if (queryResult == null)
            {
                return null;
            }

            return Score.FromEntity(queryResult);

        }

        public void SaveScore(Score score)
        {
            Store.Upsert(ScoreAsEntity(score));
        }

        public void SaveScores(IEnumerable<Score> scores)
        {
            Store.Upsert(scores.Select(s => ScoreAsEntity(s)));
        }

        public int CountBetterScores(float playerScore, string game, string level, int limit)
        {
            Query query = new(nameof(Score))
            {
                Filter = Filter.And(
                    Filter.Equal(nameof(Score.Game), game),
                    Filter.Equal(nameof(Score.Level), level),
                    Filter.LessThan(nameof(Score.Value), playerScore)
                ),
                Limit = limit,
                Projection = { "__key__" }
            };
            return Store.RunQuery(query).Entities.Count;
        }

        public IEnumerable<Score?> GetScores(string game, string level, int numResults, int skip, bool sortAscending)
        {
            var sort = sortAscending ? PropertyOrder.Types.Direction.Ascending : PropertyOrder.Types.Direction.Descending;
            Query query = new(nameof(Score))
            {
                Filter = Filter.And(
                    Filter.Equal(nameof(Score.Game), game),
                    Filter.Equal(nameof(Score.Level), level)),
                Limit = numResults,
                Order = { { nameof(Score.Value), sort } },
                Offset = skip
            };

            var results = Store.RunQuery(query);
            return results.Entities.Select(e => Score.FromEntity(e));
        }

        private Key GetScoreKey(string game, string playerId, string level)
        {
            return Store.CreateKeyFactory(nameof(Score)).CreateKey($"{game},{playerId},{level}");
        }

        private Entity ScoreAsEntity(Score score)
        {
            return new Entity()
            {
                Key = GetScoreKey(score.Game, score.PlayerId, score.Level),
                [nameof(Score.Game)] = score.Game,
                [nameof(Score.PlayerId)] = score.PlayerId,
                [nameof(Score.PlayerDisplayName)] = score.PlayerDisplayName,
                [nameof(Score.Level)] = score.Level,
                [nameof(Score.Value)] = score.Value
            };
        }

        public IEnumerable<Score> GetAllPlayerScores(string game, string playerId)
        {
            Query query = new(nameof(Score))
            {
                Filter = Filter.And(
                    Filter.Equal(nameof(Score.Game), game),
                    Filter.Equal(nameof(Score.PlayerId), playerId))
            };

            return Store.RunQuery(query).Entities.Select(e => Score.FromEntity(e));
        }
    }
}