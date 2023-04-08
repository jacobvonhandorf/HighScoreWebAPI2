namespace HighScoreWebAPI2.Services
{
    public class HashService : IHashService
    {
        public ulong CalculateHash(string read)
        {
            ulong hashedValue = 3074457345618258791ul;
            for (int i = 0; i < read.Length; i++)
            {
                hashedValue += read[i];
                hashedValue *= 3074457345618258799ul;
            }
            return hashedValue;
        }

        public bool ValidateHash(string game, string level, string playerId, string displayName, float value, ulong hash)
        {
            return hash == CalculateHash(string.Format("{0}{1}{2}{3}{4}", game, level, playerId, displayName, value));
        }
    }
}