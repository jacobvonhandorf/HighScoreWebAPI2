using HighScoreWebAPI.Controllers;
using HighScoreWebAPI2.Models;
using HighScoreWebAPI2.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighScoreWebApiTests
{
    public class ScoreControllerTests
    {
        private readonly Mock<ILogger<ScoreController>> _loggerMock = new();
        private readonly Mock<IDatabaseService> _databaseMock = new();

        private readonly ScoreController _scoreController;

        public ScoreControllerTests()
        {
            _scoreController = new ScoreController(_loggerMock.Object, _databaseMock.Object);
        }

        [Fact]
        public void Get_ScoreExists_ReturnsClientScore()
        {
            // Arrange
            string game = "game1";
            string level = "level1";
            string playerId = "player1";
            Score score = new() { Game = game, Level = level, PlayerId = playerId, Value = 100 };
            _databaseMock
                .Setup(x => x.GetScoreForPlayer(game, level, playerId))
                .Returns(score);

            // Act
            ClientScore? result = _scoreController.Get(game, level, playerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(score.AsClientScore(), result);
        }

        [Fact]
        public void Get_ScoreDoesNotExist_ReturnsNull()
        {
            // Arrange
            string game = "game1";
            string level = "level1";
            string playerId = "player1";
            _databaseMock
                .Setup(x => x.GetScoreForPlayer(game, level, playerId))
                .Returns((Score?)null);

            // Act
            ClientScore? result = _scoreController.Get(game, level, playerId);

            // Assert
            Assert.Null(result);
        }
    }

}
