using ChuckNorrisApi.Models;
using ChuckNorrisApi.Repository;
using ChuckNorrisApi.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ChuckNorrisApi.Tests
{
    //Unit test sampling, obviously not exhaustive
    public class ChuckNorrisServiceTests
    {
        private readonly Mock<IOptions<ChuckNorrisApiSettings>> _settingsMock;
        private readonly Mock<ILogger<ChuckNorrisService>> _loggerMock;
        private readonly Mock<IChuckNorrisRepository> _cnRepoMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;

        private readonly ChuckNorrisService _sut;

        public ChuckNorrisServiceTests()
        {
            _settingsMock = new Mock<IOptions<ChuckNorrisApiSettings>>();
            _loggerMock = new Mock<ILogger<ChuckNorrisService>>();
            _cnRepoMock = new Mock<IChuckNorrisRepository>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();

            _sut = new ChuckNorrisService(_settingsMock.Object, _loggerMock.Object, _cnRepoMock.Object, _httpClientFactoryMock.Object);
        }


        [Fact]
        public async Task SaveJoke_ThrowsArgumentException_WhenJokeIsNull()
        {
            //arrange
            ChuckNorrisSavedJoke joke = null;

            //act
            var expectedException = await Assert.ThrowsAsync<ArgumentException>(async () => await _sut.SaveJoke(joke));

            //assert
            _loggerMock.VerifyWarningWasCalled();

            _cnRepoMock.Verify(r => r.Save(It.IsAny<ChuckNorrisSavedJoke>()), Times.Never());
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "joke")]
        [InlineData("abc123", "")]
        [InlineData("abc123", null)]
        [InlineData(null, "joke")]
        [InlineData("", null)]
        [InlineData("  ", null)]
        [InlineData(null, "")]
        [InlineData(null, "       ")]
        [InlineData("  ", "")]
        [InlineData("", "       ")]
        [InlineData("  ", "  ")]
        public async Task SaveJoke_ThrowsArgumentException_WhenJokeMissingValues(string id, string value)
        {
            //arrange
            ChuckNorrisSavedJoke joke = new ChuckNorrisSavedJoke
            {
                Id = id,
                Value = value
            };

            //act
            var expectedException = await Assert.ThrowsAsync<ArgumentException>(async () => await _sut.SaveJoke(joke));

            //assert
            _loggerMock.VerifyWarningWasCalled();

            _cnRepoMock.Verify(r => r.Save(It.IsAny<ChuckNorrisSavedJoke>()), Times.Never());
        }

        [Theory]
        [InlineData("I am a string id, how are you?", "Funny joke here.  Sooo funny.   ha.")]
        [InlineData("What's brown and sticky? A stick.", "a")]
        [InlineData("1", "If you're an American when you go into the bathroom and American when you come out of the bathroom. What are you when you're in the bathroom? European.")]
        public async Task SaveJoke_Succeeds_WithValidStrings(string id, string value)
        {
            //arrange
            ChuckNorrisSavedJoke joke = new ChuckNorrisSavedJoke
            {
                Id = id,
                Value = value
            };

            //act
            var result = await _sut.SaveJoke(joke);

            //assert
            _cnRepoMock.Verify(r => r.Save(It.IsAny<ChuckNorrisSavedJoke>()), Times.Once());
        }
    }
}
