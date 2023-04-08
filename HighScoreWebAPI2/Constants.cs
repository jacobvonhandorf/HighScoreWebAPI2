namespace HighScoreWebAPI2
{
    public static class Constants
    {
        public static class Collections
        {
            public const string score = "Score";
        }

        public static class Database
        {
            public const string defaultNamespace = "leaderboard-api";
            public const string testNamespace = "test";
        }

        public static class MaxParameterSize
        {
            public const int maxPlayerIdSize = 32;
            public const int maxDisplayNameSize = 16;
        }

        public const string GcpProjectName = "bubbly-stone-272515";
    }
}
