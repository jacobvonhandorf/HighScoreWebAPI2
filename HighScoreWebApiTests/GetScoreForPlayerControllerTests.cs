using FluentAssertions;
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
    public class GetScoreForPlayerControllerTests
    {
        private Mock<IDatabaseService> _mockDatabase;
        private GetScoreForPlayerController _controller;

        public GetScoreForPlayerControllerTests()
        {
            _mockDatabase = new Mock<IDatabaseService>();
            _controller = new GetScoreForPlayerController(Mock.Of<ILogger<GetScoreForPlayerController>>(), _mockDatabase.Object);
        }

        [Fact]
        public void Get_ReturnsMaxValue_WhenScoreIsNull()
        {
            // Arrange
            Score? score = null;
            _mockDatabase.Setup(db => db.GetScoreForPlayer("game1", "level1", "player1")).Returns(score);

            // Act
            var result = _controller.Get("game1", "level1", "player1");

            // Assert
            result.Should().Be(float.MaxValue);
        }

        [Fact]
        public void Get_ReturnsScoreValue_WhenScoreIsNotNull()
        {
            // Arrange
            Score score = new() { Value = 100.0f };
            _mockDatabase.Setup(db => db.GetScoreForPlayer("game1", "level1", "player1")).Returns(score);

            // Act
            var result = _controller.Get("game1", "level1", "player1");

            // Assert
            result.Should().Be(score.Value);
        }

    }
}
