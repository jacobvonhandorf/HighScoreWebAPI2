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
    [Route("GetLevelScores")]
    public class LevelScoreController : ControllerBase
    {
        private const int maxResults = 50;
        private readonly ILogger<LevelScoreController> _logger;
        private readonly IDatabaseService _database;

        public LevelScoreController(ILogger<LevelScoreController> logger, IDatabaseService database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet]
        public List<ClientScore> Get(string game, string level, int numResults = 10, int skip = 0, bool sortAscending = false)
        {
            numResults = Math.Min(maxResults, numResults);

            IEnumerable<Score?> scores = _database.GetScores(game, level, numResults, skip, sortAscending);

            return scores.Select(s => s.AsClientScore()).ToList();
        }
    }
}
