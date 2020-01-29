using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using Parser.Contracts;
using Parser.Models;

namespace Parser.Services
{
    public class RssFeedService: IRssFeedService
    {
        private readonly IRssFeedRepository _rssFeedRepository;
        private readonly IAudioFileRepository _fileRepository;

        public RssFeedService(IRssFeedRepository rssFeedRepository, IAudioFileRepository fileRepository)
        {
            _rssFeedRepository = rssFeedRepository;
            _fileRepository = fileRepository;
        }   

        public async Task<List<ParsedEpisodeInfo>> ParseRssFeedAsync(string url)
        {
            var feed = _rssFeedRepository.CallRssFeed(url);
            var parsedEpisodes = new List<ParsedEpisodeInfo>();
           
            //TODO: send parallel requests in batches
            foreach (var item in feed.Items)
            {
                parsedEpisodes.Add(await MapRssFeedResponseAsync(item));
            }

            return parsedEpisodes;

        }

        private async Task<ParsedEpisodeInfo> MapRssFeedResponseAsync(SyndicationItem item)
        {
            return new ParsedEpisodeInfo
            {
                CheckSum = await _fileRepository.CalculateFileCheckSumAsync(item.Links.FirstOrDefault(link =>
                    link.MediaType == "audio/mpeg")?.Uri.AbsoluteUri),
                Title = item.Title.Text,
                Url = item.Links.FirstOrDefault(link => link.RelationshipType == "alternate")?.Uri.AbsoluteUri
            };
        }
    }
}
