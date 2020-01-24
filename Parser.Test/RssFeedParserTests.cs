using Moq;
using Parser.Contracts;
using Parser.Services;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using Xunit;

namespace Parser.Test
{
    public class RssFeedParserTests
    {
        private Mock<IRssFeedRepository> _rssFeedRepo;

        public RssFeedParserTests()
        {
            _rssFeedRepo = new Mock<IRssFeedRepository>();
        }

        [Fact]
        public void ParseValidRssFeed_ShouldReturnValidObject()
        {
            var feedMock = new SyndicationFeed { Title = new TextSyndicationContent("test", TextSyndicationContentKind.Plaintext), BaseUri = new Uri("//testUri.com") };
            feedMock.Items = new List<SyndicationItem> {
                mockSyndicationItem("title", "audio/mpeg", "alternate", "//episodeuri.test.com", 111111),
                mockSyndicationItem("title", "audio/mpeg", "alternate", "//episodeuri.test.com", 222222),
                mockSyndicationItem("title", "audio/mpeg", "alternate", "//episodeuri.test.com", 333333),
                mockSyndicationItem("title", "audio/mpeg", "alternate", "//episodeuri.test.com", 444444)
            };
            _rssFeedRepo.Setup(x => x.CallRssFeed("test")).Returns(feedMock);

            var service = new RssFeedService(_rssFeedRepo.Object);
            var result = service.GetRssFeed("test");

            Assert.NotNull(result[0]);
            Assert.NotNull(result[1]);
            Assert.NotNull(result[2]);
            Assert.NotNull(result[3]);
        }

        [Fact]
        public void ParseInvalidRssFeed_ShouldThrow()
        {
            var feedMock = new SyndicationFeed { Title = new TextSyndicationContent("test", TextSyndicationContentKind.Plaintext), BaseUri = new Uri("//testUri.com") };
            feedMock.Items = new List<SyndicationItem> {
                mockSyndicationItem("title", "audio/mpeg", "incorrectInput", "//episodeuri.test.com", 111111)
            };
            _rssFeedRepo.Setup(x => x.CallRssFeed("test")).Returns(feedMock);

            var service = new RssFeedService(_rssFeedRepo.Object);

            Assert.Throws<InvalidOperationException>(() => { service.GetRssFeed("test"); });
        }

        private SyndicationItem mockSyndicationItem(string title, string mediaType, string relationShipType, string episodeUrl, long length)
        {
            var item = new SyndicationItem { Title = new TextSyndicationContent(title, TextSyndicationContentKind.Plaintext) };
            item.Links.Add(new SyndicationLink { MediaType = mediaType, Length = length });
            item.Links.Add(new SyndicationLink { RelationshipType = relationShipType, Uri = new Uri(episodeUrl) });
            return item;
        }
    }
}
