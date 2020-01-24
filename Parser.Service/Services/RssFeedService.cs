using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using Parser.Contracts;
using Parser.Models;

namespace Parser.Services
{
    public class RssFeedService: IRssFeedService
    {
        private readonly IRssFeedRepository _rssFeedRepository;

        public RssFeedService(IRssFeedRepository rssFeedRepository)
        {
            _rssFeedRepository = rssFeedRepository;
        }   

        public List<ParsedEpisodeInfo> GetRssFeed(string url)
        {
            var feed = _rssFeedRepository.CallRssFeed(url);
            return feed.Items.Select(item =>
               MapRssFeedResponse(item)).ToList();
        }

        public ParsedEpisodeInfo MapRssFeedResponse(SyndicationItem item)
        {
            
            try
            {
                return new ParsedEpisodeInfo
                {
                    CheckSum = item.Links.FirstOrDefault(link => link.MediaType == "audio/mpeg").Length,
                    Title = item.Title.Text,
                    Url = item.Links.FirstOrDefault(link => link.RelationshipType == "alternate").Uri.AbsoluteUri
                };

            }catch(Exception e)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
