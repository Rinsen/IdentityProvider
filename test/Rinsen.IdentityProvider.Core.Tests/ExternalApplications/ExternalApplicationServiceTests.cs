using Moq;
using System;
using Xunit;

namespace Rinsen.IdentityProvider.Core.ExternalApplications
{
    public class ExternalApplicationServiceTests
    {



        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WhenNoHostIsProvided_GetFailedValidationResultAndNullToken(string data)
        {

            var externalApplicationService = new ExternalApplicationService(null, null, StubLogger.CreateLogger<ExternalApplicationService>());

            var result = externalApplicationService.GetTokenForValidHostAsync(data, Guid.Empty).Result;

            Assert.True(result.Failed);
            Assert.False(result.Succeeded);
            Assert.Null(result.Token);
        }

        [Fact]
        public void WhenHostIsNotFound_GetFailedValidationResultAndNullToken()
        {
            var externalApplicationStorageMock = new Mock<IExternalApplicationStorage>();

            var externalApplicationService = new ExternalApplicationService(externalApplicationStorageMock.Object, null, StubLogger.CreateLogger<ExternalApplicationService>());

            var result = externalApplicationService.GetTokenForValidHostAsync("www.rinsen.se", Guid.Empty).Result;

            Assert.True(result.Failed);
            Assert.False(result.Succeeded);
            Assert.Null(result.Token);
            externalApplicationStorageMock.Verify(m => m.GetFromHostAsync(It.Is<string>(host => host == "www.rinsen.se")), Times.Once);
        }

        [Fact]
        public void WhenHostIsFound_GetSucceededValidationResultAndToken()
        {
            var identity = Guid.NewGuid();
            var externalApplicationId = Guid.NewGuid();

            var externalApplicationStorageMock = new Mock<IExternalApplicationStorage>();
            externalApplicationStorageMock.Setup(m => m.GetFromHostAsync(It.Is<string>(host => host == "www.rinsen.se")))
                .ReturnsAsync(new ExternalApplication
                {
                    ExternalApplicationId = externalApplicationId
                });

            var tokenStorageMock = new Mock<ITokenStorage>();

            var externalApplicationService = new ExternalApplicationService(externalApplicationStorageMock.Object, tokenStorageMock.Object, StubLogger.CreateLogger<ExternalApplicationService>());

            var result = externalApplicationService.GetTokenForValidHostAsync("www.rinsen.se", Guid.Empty).Result;

            tokenStorageMock.Verify(mock => mock.CreateAsync(It.Is<Token>(token => token.TokenId == result.Token && token.ExternalApplicationId == externalApplicationId && token.IdentityId == Guid.Empty)), Times.Once);
            Assert.True(result.Token.Length >= 40);
            Assert.False(result.Failed);
            Assert.True(result.Succeeded);
        }


    }
}
