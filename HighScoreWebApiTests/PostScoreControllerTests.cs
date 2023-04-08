using HighScoreWebAPI.Controllers;
using HighScoreWebAPI;
using HighScoreWebAPI2.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HighScoreWebAPI2.Models;

namespace HighScoreWebApiTests
{
    public class PostScoreControllerTests
    {
        private readonly Mock<ILogger<PostScoreController>> _loggerMock = new();
        private readonly Mock<IDatabaseService> _databaseMock = new();
        private readonly Mock<IHashService> _hashServiceMock = new();
        private readonly Mock<IUsernameValidationService> _usernameValidationServiceMock = new();

        private readonly PostScoreController _controller;

        public PostScoreControllerTests()
        {
            _controller = new PostScoreController(_loggerMock.Object, _databaseMock.Object, _hashServiceMock.Object, _usernameValidationServiceMock.Object);
        }

        [Theory]
        [InlineData("game1", "level1", "player1", "displayName1", 1.0f, true)]
        [InlineData("game2", "level2", "player2", "displayName2", 2.5f, true)]
        public void Post_ValidInput_Success(string game, string level, string playerId, string displayName, float value, bool isValidUsername)
        {
            // Arrange
            _usernameValidationServiceMock.Setup(x => x.ValidateUsername(displayName))
                .Returns(new ValidateUsernameResult { IsValid = isValidUsername });

            _hashServiceMock.Setup(x => x.ValidateHash(game, level, playerId, displayName, value, 1234))
                .Returns(true);

            // Act
            var result = _controller.Post(game, level, playerId, displayName, value, 1234);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();

            _databaseMock.Verify(x => x.SaveScore(It.IsAny<Score>()), Times.Once);
        }

        [Theory]
        [InlineData("game1", "level1", "player1", "displayName1", 1.0f, true, false, "Username contains invalid characters", false)]
        [InlineData("game2", "level2", "player2", "displayName2", 2.5f, false, true, "Unexpected Error", false)]
        [InlineData("game3", "level3", "player3", "displayName3", 3.5f, true, true, "Unexpected Error", true)]
        public void Post_InvalidInput_Failure(
            string game, string level, string playerId, string displayName, float value, bool hashIsValid, bool isValidUsername, string invalidReason, bool idIsInvalid)
        {
            // Arrange
            _usernameValidationServiceMock.Setup(x => x.ValidateUsername(displayName))
                .Returns(new ValidateUsernameResult { IsValid = isValidUsername, InvalidReason = invalidReason });
            _hashServiceMock.Setup(x => x.ValidateHash(game, level, playerId, displayName, value, 1234))
                .Returns(hashIsValid);

            if (idIsInvalid)
            {
                playerId = new string('a', HighScoreWebAPI2.Constants.MaxParameterSize.maxPlayerIdSize + 1);
            }

            // Act
            var result = _controller.Post(game, level, playerId, displayName, value, 1234);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.FailureReason.Should().Be(invalidReason);

            _databaseMock.Verify(x => x.SaveScore(It.IsAny<Score>()), Times.Never);
        }
    }
}
