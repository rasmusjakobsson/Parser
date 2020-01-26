using Microsoft.AspNetCore.Mvc;
using Moq;
using RssFeed.Api.Controllers;
using Parser.Contracts;
using System.Collections.Generic;
using Xunit;

namespace Parser.Test
{
    public class ControllerTests
    {
        private readonly Mock<IRssFeedService> _rssFeedService;

        public ControllerTests()
        {
            _rssFeedService = new Mock<IRssFeedService>();
        }

        [Fact]
        public void CallApiWithCorrectParameter_ShouldReturnOk()
        {
            _rssFeedService.Setup(x => x.GetRssFeed("testUrl")).Returns(
                new List<Models.ParsedEpisodeInfo> {
                    new Models.ParsedEpisodeInfo { CheckSum = "testCheckSum", Title = "TestEpisode", Url = "http://test.se" }
                });

            var apiController = new RssFeedController(_rssFeedService.Object);
            var result = apiController.Get("test");
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void CallApiWithNoParameter_ShouldReturnBadRequest()
        {
            _rssFeedService.Setup(x => x.GetRssFeed("testUrl")).Returns(
                It.IsAny<List<Models.ParsedEpisodeInfo>>());

            var apiController = new RssFeedController(_rssFeedService.Object);
            var result = apiController.Get("");
            Assert.IsType<BadRequestResult>(result.Result);
        }
    }
}
