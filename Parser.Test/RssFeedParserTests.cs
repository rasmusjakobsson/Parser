using Moq;
using Parser.Contracts;
using Parser.Services;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using Parser.Models;
using Xunit;

namespace Parser.Test
{
    public class RssFeedParserTests
    {
        private readonly Mock<IRssFeedRepository> _rssFeedRepo;
        private readonly Mock<IShowRepository> _showRepositoryMock;

        public RssFeedParserTests()
        {
            _rssFeedRepo = new Mock<IRssFeedRepository>();
            _showRepositoryMock = new Mock<IShowRepository>();

            var showResponseMock = new ShowResponse
            {
                episodes = new List<Episode>
                {
                    new Episode {id = "TestEpiId1", checkSum = "CheckSum1"},
                    new Episode {id = "TestEpiId2", checkSum = "CheckSum2"}
                }
            };

            _showRepositoryMock.Setup(x => x.GetShowInfo("testShowId")).Returns(showResponseMock);
        }

        [Fact]
        public void ParseValidRssFeed_ShouldReturnValidObject()
        {
            var feedMock = new SyndicationFeed { Id = "testShowId",Title = new TextSyndicationContent("test", TextSyndicationContentKind.Plaintext), BaseUri = new Uri("//testUri.com") };
            feedMock.Items = new List<SyndicationItem> {
                mockSyndicationItem("TestEpiId1", "title", "audio/mpeg", "alternate", "//episodeuri.test.com", 111111),
                mockSyndicationItem("TestEpiId2", "title", "audio/mpeg", "alternate", "//episodeuri.test.com", 222222),
            };
            feedMock.ElementExtensions.Add(new SyndicationElementExtension("showId", "", "testShowId"));

            
            _rssFeedRepo.Setup(x => x.CallRssFeed("test")).Returns(feedMock);

            var service = new RssFeedService(_rssFeedRepo.Object, _showRepositoryMock.Object);
            var result = service.GetRssFeed("test");

            Assert.NotNull(result[0]);
            Assert.NotNull(result[1]);
        }

        [Fact]
        public void ParseInvalidRssFeed_ShouldThrow()
        {
            var feedMock = new SyndicationFeed {Title = new TextSyndicationContent("test", TextSyndicationContentKind.Plaintext), BaseUri = new Uri("//testUri.com") };
            feedMock.Items = new List<SyndicationItem> {
                mockSyndicationItem("TestEpiId1","title", "audio/mpeg", "incorrectInput", "//episodeuri.test.com", 111111)
            };
            feedMock.ElementExtensions.Add(new SyndicationElementExtension("showId", "", "testShowId"));
            _rssFeedRepo.Setup(x => x.CallRssFeed("test")).Returns(feedMock);

            var service = new RssFeedService(_rssFeedRepo.Object, _showRepositoryMock.Object);
            var result = service.GetRssFeed("test");

            Assert.Null(result[0].Url);
        }

        private SyndicationItem mockSyndicationItem(string episodeId, string title, string mediaType, string relationShipType, string episodeUrl, long length)
        {
            var item = new SyndicationItem { Id = episodeId, Title = new TextSyndicationContent(title, TextSyndicationContentKind.Plaintext) };
            item.Links.Add(new SyndicationLink { MediaType = mediaType, Length = length });
            item.Links.Add(new SyndicationLink { RelationshipType = relationShipType, Uri = new Uri(episodeUrl) });
            return item;
        }
    }
}
