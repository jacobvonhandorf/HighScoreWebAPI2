namespace HighScoreWebAPI2.Models
{
    public class ValidateUsernameResult
    {
        public bool IsValid { get; set; }
        public string? InvalidReason { get; set; }
    }
}
