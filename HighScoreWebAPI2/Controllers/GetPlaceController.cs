using Google.Cloud.Datastore.V1;
using HighScoreWebAPI2.Models;
using HighScoreWebAPI2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HighScoreWebAPI.Controllers
{
    [ApiController]
    [Route("GetPlace")]
    public class GetPlaceController : ControllerBase
    {
        public const int limit = 1000;
        private readonly ILogger<GetPlaceController> _logger;
        private readonly IDatabaseService _database;

        public GetPlaceController(ILogger<GetPlaceController> logger, IDatabaseService database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet]
        public string Get(string game, string level, string playerId)
        {
            Score? score = _database.GetScoreForPlayer(playerId, game, level);
            float playerScore = score != null ? score.Value : float.MaxValue;

            int betterScores = _database.CountBetterScores(playerScore, game, level, limit);

            int place = betterScores + 1;
            return place > limit ? $"> {limit}" : place.ToString();
        }
    }
}
