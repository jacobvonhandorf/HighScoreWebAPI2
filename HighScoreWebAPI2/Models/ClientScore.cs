namespace HighScoreWebAPI2.Models
{
    public class ClientScore
    {
        public string Game { get; set; }
        public string PlayerDisplayName { get; set; }
        public string Level { get; set; }
        public float Value { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ClientScore score &&
                   Game == score.Game &&
                   PlayerDisplayName == score.PlayerDisplayName &&
                   Level == score.Level &&
                   Value == score.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Game, PlayerDisplayName, Level, Value);
        }
    }
}