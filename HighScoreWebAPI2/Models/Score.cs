using Google.Cloud.Datastore.V1;
using HighScoreWebAPI2;
using System;

namespace HighScoreWebAPI2.Models
{
    public class Score
    {
        public string Game { get; set; }
        public string PlayerId { get; set; }
        public string PlayerDisplayName { get; set; }
        public string Level { get; set; }
        public float Value { get; set; }

        public ClientScore AsClientScore()
        {
            return new ClientScore()
            {
                Game = Game,
                Level = Level,
                PlayerDisplayName = PlayerDisplayName,
                Value = Value
            };
        }

        public static Score? FromEntity(Entity e)
        {
            if (e == null)
                return null;

            return new Score()
            {
                Game = e.Properties[nameof(Game)].StringValue.ToString(),
                Level = e.Properties[nameof(Level)].StringValue.ToString(),
                PlayerDisplayName = e.Properties[nameof(PlayerDisplayName)].StringValue.ToString(),
                PlayerId = e.Properties[nameof(PlayerId)].StringValue.ToString(),
                Value = (float)e.Properties[nameof(Value)].DoubleValue
            };
        }

    }
}