namespace HighScoreWebAPI2.Services
{
    public interface IHashService
    {
        bool ValidateHash(string game, string level, string playerId, string displayName, float value, ulong hash);
    }
}