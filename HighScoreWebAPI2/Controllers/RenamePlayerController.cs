using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using HighScoreWebAPI2.Services;
using HighScoreWebAPI2.Models;

namespace HighScoreWebAPI.Controllers
{
    [ApiController]
    [Route("RenamePlayer")]
    public class RenamePlayerController : ControllerBase
    {
        private readonly ILogger<RenamePlayerController> _logger;
        private readonly IDatabaseService _database;
        private readonly IUsernameValidationService _usernameValidationService;

        public RenamePlayerController(ILogger<RenamePlayerController> logger, IDatabaseService database, IUsernameValidationService usernameValidationService)
        {
            _logger = logger;
            _database = database;
            _usernameValidationService = usernameValidationService;
        }

        [HttpGet]
        public ValidateUsernameResult Post(string game, string playerId, string newDisplayName)
        {
            ValidateUsernameResult usernameValidation = _usernameValidationService.ValidateUsername(newDisplayName);
            if (!usernameValidation.IsValid)
            {
                return usernameValidation;
            }

            IEnumerable<Score> scoresToUpdate = _database.GetAllPlayerScores(game, playerId);
            foreach (var e in scoresToUpdate)
            {
                e.PlayerDisplayName = newDisplayName;
            }
            _database.SaveScores(scoresToUpdate);

            _logger.LogInformation("{0} changed their display name to {1}", playerId, newDisplayName);

            return usernameValidation;
        }
    }
}
