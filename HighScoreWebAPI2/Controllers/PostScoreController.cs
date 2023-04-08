using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ProfanityFilter;
using Google.Cloud.Datastore.V1;
using HighScoreWebAPI2;
using HighScoreWebAPI2.Services;
using HighScoreWebAPI2.Models;

namespace HighScoreWebAPI.Controllers
{
    [ApiController]
    [Route("PostScore")]
    public class PostScoreController : ControllerBase
    {
        private readonly ILogger<PostScoreController> _logger;
        private readonly IDatabaseService _database;
        private readonly IHashService _hashService;
        private readonly IUsernameValidationService _usernameValidationService;

        public PostScoreController(ILogger<PostScoreController> logger, IDatabaseService database, IHashService hashService, IUsernameValidationService usernameValidationService)
        {
            _logger = logger;
            _database = database;
            _hashService = hashService;
            _usernameValidationService = usernameValidationService;
        }

        [HttpPost]
        public PostScoreResult Post(string game, string level, string playerId, string displayName, float value, ulong misc)
        {
            ValidateUsernameResult usernameValidation = _usernameValidationService.ValidateUsername(displayName);

            if (!usernameValidation.IsValid)
            {
                return new PostScoreResult()
                {
                    FailureReason = usernameValidation.InvalidReason,
                    Success = false
                };
            }

            bool idIsInvalid = playerId.Length > Constants.MaxParameterSize.maxPlayerIdSize;
            if (idIsInvalid)
            {
                return new PostScoreResult()
                {
                    FailureReason = "Unexpected Error",
                    Success = false
                };
            }

            if (!_hashService.ValidateHash(game, level, playerId, displayName, value, misc))
            {
                return new PostScoreResult()
                {
                    FailureReason = "Unexpected Error",
                    Success = false
                };
            }

            SaveScore(game, level, playerId, displayName, value);

            return new PostScoreResult() { Success = true };
        }

        private void SaveScore(string game, string level, string playerId, string displayName, float value)
        {
            _database.SaveScore(new Score()
            {
                Game = game,
                Level = level,
                PlayerDisplayName = displayName,
                PlayerId = playerId,
                Value = value
            });
            _logger.LogInformation("New score posted game: {0} level: {1} playerId: {2} displayName: {3} value: {4}", game, level, playerId, displayName, value);
        }
    }
}
