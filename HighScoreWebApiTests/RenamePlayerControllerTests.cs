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
    public class RenamePlayerControllerTests
    {
        private readonly Mock<ILogger<RenamePlayerController>> _loggerMock = new();
        private readonly Mock<IDatabaseService> _databaseMock = new();
        private readonly Mock<IUsernameValidationService> _usernameValidationServiceMock = new();

        private readonly RenamePlayerController _renamePlayerController;

        public RenamePlayerControllerTests()
        {
            _renamePlayerController = new RenamePlayerController(_loggerMock.Object, _databaseMock.Object, _usernameValidationServiceMock.Object);
        }

        [Fact]
        public void Post_ValidUsername_UpdatesPlayerDisplayNameAndReturnsValidResult()
        {
            // Arrange
            string game = "game1";
            string playerId = "player1";
            string newDisplayName = "newDisplayName";
            ValidateUsernameResult usernameValidationResult = new() { IsValid = true };
            IEnumerable<Score> scores = new List<Score> { new Score { PlayerId = playerId, PlayerDisplayName = "oldDisplayName" } };
            _usernameValidationServiceMock
                .Setup(u => u.ValidateUsername(newDisplayName))
                .Returns(usernameValidationResult);
            _databaseMock
                .Setup(d => d.GetAllPlayerScores(game, playerId))
                .Returns(scores);

            // Act
            ValidateUsernameResult result = _renamePlayerController.Post(game, playerId, newDisplayName);

            // Assert
            Assert.Equal(usernameValidationResult, result);
            Assert.Equal(newDisplayName, scores.First().PlayerDisplayName);
            _databaseMock.Verify(d => d.SaveScores(scores), Times.Once);
        }

        [Fact]
        public void Post_InvalidUsername_ReturnsInvalidResult()
        {
            // Arrange
            string game = "game1";
            string playerId = "player1";
            string newDisplayName = "newDisplayName";
            ValidateUsernameResult usernameValidationResult = new() { IsValid = false };
            _usernameValidationServiceMock
                .Setup(u => u.ValidateUsername(newDisplayName))
                .Returns(usernameValidationResult);

            // Act
            ValidateUsernameResult result = _renamePlayerController.Post(game, playerId, newDisplayName);

            // Assert
            Assert.Equal(usernameValidationResult, result);
            _databaseMock.Verify(d => d.GetAllPlayerScores(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _databaseMock.Verify(d => d.SaveScores(It.IsAny<IEnumerable<Score>>()), Times.Never);
        }
    }

}
