using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml.Linq;
using Parser.Contracts;
using Parser.Models;

namespace Parser.Services
{
    public class RssFeedService: IRssFeedService
    {
        private readonly IRssFeedRepository _rssFeedRepository;
        private readonly IShowRepository _showRepository;

        public RssFeedService(IRssFeedRepository rssFeedRepository, IShowRepository showRepository)
        {
            _rssFeedRepository = rssFeedRepository;
            _showRepository = showRepository;
        }   

        public List<ParsedEpisodeInfo> GetRssFeed(string url)
        {
            var feed = _rssFeedRepository.CallRssFeed(url);
            var showInfo = _showRepository.GetShowInfo(GetShowId(feed));

            return feed.Items.Select(item => MapRssFeedResponse(item, showInfo.episodes.FirstOrDefault(epi => epi.id == item.Id)?.checkSum)).ToList();
        }

        private static ParsedEpisodeInfo MapRssFeedResponse(SyndicationItem item, string checkSum)
        {
            try
            {
                return new ParsedEpisodeInfo
                {
                    CheckSum = checkSum,
                    Title = item.Title.Text,
                    Url = item.Links.FirstOrDefault(link => link.RelationshipType == "alternate")?.Uri.AbsoluteUri
                };

            }catch(Exception e)
            {
                throw new InvalidOperationException();
            }
        }

        private static string GetShowId(SyndicationFeed feed)
        {
            var showIdElement = feed.ElementExtensions.Where((x => x.OuterName == "showId")).FirstOrDefault()
                ?.GetObject<XElement>();
            return showIdElement?.Value;
        }
    }
}
