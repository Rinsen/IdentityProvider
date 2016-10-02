using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Rinsen.IdentityProvider.Core.ExternalApplications
{
    public class ExternalApplicationServiceTests
    {

        [Fact]
        public void WhenHostIsNotFound_GetFailedValidationResultAndNullToken()
        {
            var externalApplicationStorageMock = new Mock<IExternalApplicationStorage>();

            var externalApplicationService = new ExternalApplicationService(externalApplicationStorageMock.Object, null, null);

            var result = externalApplicationService.GetTokenForValidHostAsync("http://www.rinsen.se/Example").Result;

            Assert.True(result.Failed);
            Assert.False(result.Succeeded);
            Assert.Null(result.Token);
            externalApplicationStorageMock.Verify(m => m.GetAsync(It.Is<string>(host => host == "www.rinsen.se")), Times.Once);
        }

        [Fact]
        public void WhenHostIsFound_GetSucceededValidationResultAndToken()
        {
            var identity = Guid.NewGuid();
            var externalApplicationId = Guid.NewGuid();

            var externalApplicationStorageMock = new Mock<IExternalApplicationStorage>();
            externalApplicationStorageMock.Setup(m => m.GetAsync(It.Is<string>(host => host == "www.rinsen.se")))
                .ReturnsAsync(new ExternalApplication
                {
                    ExternalApplicationId = externalApplicationId
                });

            var tokenStorageMock = new Mock<ITokenStorage>();
            var identityAccessorMock = new Mock<IIdentityAccessor>();
            identityAccessorMock.Setup(m => m.IdentityId).Returns(identity);

            var externalApplicationService = new ExternalApplicationService(externalApplicationStorageMock.Object, tokenStorageMock.Object, identityAccessorMock.Object);

            var result = externalApplicationService.GetTokenForValidHostAsync("http://www.rinsen.se/Example").Result;

            Assert.False(result.Failed);
            Assert.True(result.Succeeded);
            
            
        }
    }
}
