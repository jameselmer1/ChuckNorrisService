using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace ChuckNorrisApi.Tests
{
    public static class UnitTestExtensionHelpers
    {
        //used with credit https://adamstorr.azurewebsites.net/blog/mocking-ilogger-with-moq
        public static Mock<ILogger<T>> VerifyWarningWasCalled<T>(this Mock<ILogger<T>> logger)
        {
            logger.Verify(l => l.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));

            return logger;
        }
    }
}
