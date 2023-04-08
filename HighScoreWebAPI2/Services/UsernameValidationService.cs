using HighScoreWebAPI2.Models;
using ProfanityFilter;
using ProfanityFilter.Interfaces;

namespace HighScoreWebAPI2.Services
{
    public class UsernameValidationService : IUsernameValidationService
    {
        private readonly IProfanityFilter _profanityFilter;

        public UsernameValidationService(IProfanityFilter profanityFilter)
        {
            _profanityFilter = profanityFilter;
        }

        public ValidateUsernameResult ValidateUsername(string username)
        {
            bool nameIsInvalid = username.Length > Constants.MaxParameterSize.maxDisplayNameSize || _profanityFilter.ContainsProfanity(username.ToLower());
            if (nameIsInvalid)
            {
                return new ValidateUsernameResult()
                {
                    InvalidReason = "Invalid Name",
                    IsValid = false
                };
            }

            return new ValidateUsernameResult()
            {
                InvalidReason = null,
                IsValid = true
            };
        }

    }
}
