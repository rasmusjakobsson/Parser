using Microsoft.AspNetCore.Mvc;
using Moq;
using Parser.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using Parser.Api.Controllers;
using Parser.Models;
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
        public async void CallApiWithCorrectParameter_ShouldReturnOk()
        {
            _rssFeedService.Setup(x => x.ParseRssFeedAsync("testUrl")).Returns(
                new Task<List<Models.ParsedEpisodeInfo>>(() =>
                    new List<ParsedEpisodeInfo>
                    {
                        new Models.ParsedEpisodeInfo { CheckSum = "testCheckSum", Title = "TestEpisode", Url = "http://test.se" }
                    })
                );

            var apiController = new RssFeedController(_rssFeedService.Object);
            var result = await apiController.Get("test");
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void CallApiWithNoParameter_ShouldReturnBadRequest()
        {
            _rssFeedService.Setup(x => x.ParseRssFeedAsync("testUrl")).Returns(
                It.IsAny<Task<List<Models.ParsedEpisodeInfo>>>());

            var apiController = new RssFeedController(_rssFeedService.Object);
            var result = await apiController.Get("");
            Assert.IsType<BadRequestResult>(result.Result);
        }
    }
}
