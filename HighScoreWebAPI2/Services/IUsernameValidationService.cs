using HighScoreWebAPI2.Models;

namespace HighScoreWebAPI2.Services
{
    public interface IUsernameValidationService
    {
        ValidateUsernameResult ValidateUsername(string username);
    }
}