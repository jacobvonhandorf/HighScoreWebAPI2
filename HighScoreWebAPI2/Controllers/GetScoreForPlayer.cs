using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Moq;
using HighScoreWebAPI2.Services;
using HighScoreWebAPI2.Models;
using Google.Apis.Util.Store;

namespace HighScoreWebAPI.Controllers
{
    [ApiController]
    [Route("GetScoreForPlayer")]
    public class GetScoreForPlayerController : ControllerBase
    {
        private readonly ILogger<GetScoreForPlayerController> _logger;
        private readonly IDatabaseService _database;

        public GetScoreForPlayerController(ILogger<GetScoreForPlayerController> logger, IDatabaseService database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet]
        public float Get(string game, string level, string playerId)
        {
            Score? score = _database.GetScoreForPlayer(playerId, game, level);
            return score == null ? float.MaxValue : score.Value;
        }
    }
}
