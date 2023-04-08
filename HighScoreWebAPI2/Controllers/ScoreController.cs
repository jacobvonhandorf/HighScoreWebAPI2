using Google.Cloud.Datastore.V1;
using HighScoreWebAPI2.Models;
using HighScoreWebAPI2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace HighScoreWebAPI.Controllers
{
    [ApiController]
    [Route("GetScore")]
    public class ScoreController : ControllerBase
    {
        private readonly ILogger<ScoreController> _logger;
        private readonly IDatabaseService _database;

        public ScoreController(ILogger<ScoreController> logger, IDatabaseService database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet]
        public ClientScore? Get(string game, string level, string playerId)
        {
            Score? score = _database.GetScoreForPlayer(game, level, playerId);

            if (score == null)
                return null;
            else
                return score.AsClientScore();
        }
    }
}
