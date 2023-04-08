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
    public class LevelScoreControllerTests
    {
        private readonly Mock<IDatabaseService> _databaseMock = new();
        private readonly Mock<ILogger<LevelScoreController>> _loggerMock = new();
        private readonly LevelScoreController _controller;

        public LevelScoreControllerTests()
        {
            _controller = new LevelScoreController(_loggerMock.Object, _databaseMock.Object);
        }

        [Fact]
        public void Get_ReturnsExpectedNumberOfResults()
        {
            // Arrange
            var game = "TestGame";
            var level = "TestLevel";
            var numResults = 5;
            var scores = new List<Score?>
            {
                new Score { Value = 100 },
                new Score { Value = 200 },
                new Score { Value = 300 },
                new Score { Value = 400 },
                new Score { Value = 500 },
            };
            _databaseMock.Setup(d => d.GetScores(game, level, numResults, 0, false)).Returns(scores);

            // Act
            var result = _controller.Get(game, level, numResults, 0, false);

            // Assert
            result.Should().HaveCount(5);
        }

        [Fact]
        public void Get_ReturnsExpectedClientScores()
        {
            // Arrange
            var game = "TestGame";
            var level = "TestLevel";
            var numResults = 5;
            var scores = new List<Score?>
            {
                new Score { Value = 100 },
                new Score { Value = 200 },
                new Score { Value = 300 },
                new Score { Value = 400 },
                new Score { Value = 500 },
            };
            _databaseMock.Setup(d => d.GetScores(game, level, numResults, 0, false)).Returns(scores);

            // Act
            var result = _controller.Get(game, level, numResults, 0, false);

            // Assert
            result.Should().BeEquivalentTo(new List<ClientScore>
            {
                new ClientScore { Value = 100 },
                new ClientScore { Value = 200 },
                new ClientScore { Value = 300 },
                new ClientScore { Value = 400 },
                new ClientScore { Value = 500 }
            });
        }

        [Fact]
        public void Get_NumResultsGreaterThanMaxResults_ClampsNumResults()
        {
            // Arrange
            var game = "TestGame";
            var level = "TestLevel";
            var numResults = 10;
            var scores = new List<Score?>
            {
                new Score { Value = 100 },
                new Score { Value = 200 },
                new Score { Value = 300 },
                new Score { Value = 400 },
                new Score { Value = 500 }
            };
            _databaseMock.Setup(d => d.GetScores(game, level, numResults, 0, false)).Returns(scores);

            // Act
            var result = _controller.Get(game, level, numResults, 0, false);

            // Assert
            result.Should().HaveCount(scores.Count);
        }
    }
}
