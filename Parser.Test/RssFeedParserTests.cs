using Moq;
using Parser.Contracts;
using Parser.Services;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using Xunit;

namespace Parser.Test
{
    public class RssFeedParserTests
    {
        private readonly Mock<IRssFeedRepository> _rssFeedRepo;
        private readonly Mock<IAudioFileRepository> _fileRepository;

        public RssFeedParserTests()
        {
            _rssFeedRepo = new Mock<IRssFeedRepository>();
            _fileRepository = new Mock<IAudioFileRepository>();
            _fileRepository.Setup(x => x.CalculateFileCheckSumAsync("http://test.com/test1.mp3")).ReturnsAsync("testCheckSum1");
            _fileRepository.Setup(x => x.CalculateFileCheckSumAsync("http://test.com/test2.mp3")).ReturnsAsync("testCheckSum2");
        }

        [Fact]
        public async Task ParseValidRssFeed_ShouldReturnValidObject()
        {
            var feedMock = new SyndicationFeed
            {
                Id = "testShowId",
                Title = new TextSyndicationContent("test", TextSyndicationContentKind.Plaintext),
                BaseUri = new Uri("//testUri.com"),
                Items = new List<SyndicationItem>
                {
                    MockSyndicationItem("TestEpiId1", "title", "audio/mpeg", "alternate", "http://test.com/test1.mp3",
                        "http://test.com/test1"),
                    MockSyndicationItem("TestEpiId2", "title", "audio/mpeg", "alternate", "http://test.com/test2.mp3",
                        "http://test.com/test2"),
                }
            };

            _rssFeedRepo.Setup(x => x.CallRssFeed("test")).Returns(feedMock);

            var service = new RssFeedService(_rssFeedRepo.Object, _fileRepository.Object);
            var result = await service.ParseRssFeedAsync("test");
            
            Assert.NotNull(result[0].CheckSum);
            Assert.NotNull(result[1].CheckSum);
        }

        [Fact]
        public async Task ParseInvalidRssFeed_ShouldThrow()
        {
            var feedMock = new SyndicationFeed {Title = new TextSyndicationContent("test", TextSyndicationContentKind.Plaintext), BaseUri = new Uri("//testUri.com") };
            feedMock.Items = new List<SyndicationItem> {
                MockSyndicationItem("TestEpiId1", "title", "audio/mpeg", "incorrectInput", "http://test.com/test1.mp3", "http://test.com/test1"),
            };
            feedMock.ElementExtensions.Add(new SyndicationElementExtension("showId", "", "testShowId"));
            _rssFeedRepo.Setup(x => x.CallRssFeed("test")).Returns(feedMock);

            var service = new RssFeedService(_rssFeedRepo.Object,_fileRepository.Object);
            var result = await service.ParseRssFeedAsync("test");

            Assert.Null(result[0].Url);
        }

        private SyndicationItem MockSyndicationItem(string episodeId, string title, string mediaType, string relationShipType, string mp3FileUrl, string episodeUrl)
        {
            var item = new SyndicationItem { Id = episodeId, Title = new TextSyndicationContent(title, TextSyndicationContentKind.Plaintext) };
            item.Links.Add(new SyndicationLink { RelationshipType = relationShipType,  Uri = new Uri(episodeUrl)});
            item.Links.Add(new SyndicationLink { MediaType = mediaType, Uri = new Uri(mp3FileUrl) });
            return item;
        }
    }
}
