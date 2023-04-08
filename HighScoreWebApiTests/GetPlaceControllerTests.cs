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
    public class GetPlaceControllerTests
    {
        private Mock<IDatabaseService> _mockDatabase;
        private GetPlaceController _controller;

        public GetPlaceControllerTests()
        {
            _mockDatabase = new Mock<IDatabaseService>();
            _controller = new GetPlaceController(Mock.Of<ILogger<GetPlaceController>>(), _mockDatabase.Object);
        }

        [Fact]
        public void Get_ReturnsMaxValue_WhenScoreIsNull()
        {
            // Arrange
            Score? score = null;
            _mockDatabase.Setup(db => db.GetScoreForPlayer("player1", "game1", "level1")).Returns(score);
            _mockDatabase.Setup(db => db.CountBetterScores(float.MaxValue, "game1", "level1", GetPlaceController.limit)).Returns(GetPlaceController.limit - 1);

            // Act
            var result = _controller.Get("game1", "level1", "player1");

            // Assert
            result.Should().Be(GetPlaceController.limit.ToString());
        }

        [Fact]
        public void Get_ReturnsOne_WhenPlayerHasHighestScore()
        {
            // Arrange
            Score score = new() { Value = 100.0f };
            _mockDatabase.Setup(db => db.GetScoreForPlayer("player1", "game1", "level1")).Returns(score);
            _mockDatabase.Setup(db => db.CountBetterScores(score.Value, "game1", "level1", GetPlaceController.limit)).Returns(0);

            // Act
            var result = _controller.Get("game1", "level1", "player1");

            // Assert
            result.Should().Be("1");
        }

        [Fact]
        public void Get_ReturnsExpectedPlace_WhenPlayerHasLowerScore()
        {
            // Arrange
            Score score = new() { Value = 50.0f };
            _mockDatabase.Setup(db => db.GetScoreForPlayer("player1", "game1", "level1")).Returns(score);
            _mockDatabase.Setup(db => db.CountBetterScores(score.Value, "game1", "level1", GetPlaceController.limit)).Returns(2);

            // Act
            var result = _controller.Get("game1", "level1", "player1");

            // Assert
            result.Should().Be("3");
        }

        [Fact]
        public void Get_ReturnsGreaterThanLimit_WhenPlayerHasLowestScore()
        {
            // Arrange
            Score score = new()
            { Value = 10.0f };
            _mockDatabase.Setup(db => db.GetScoreForPlayer("player1", "game1", "level1")).Returns(score);
            _mockDatabase.Setup(db => db.CountBetterScores(score.Value, "game1", "level1", GetPlaceController.limit)).Returns(GetPlaceController.limit);

            // Act
            var result = _controller.Get("game1", "level1", "player1");

            // Assert
            result.Should().Be("> 1000");
        }
    }

}
