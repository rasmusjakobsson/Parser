using Parser.Models;
using System.Collections.Generic;

namespace Parser.Contracts
{
    public interface IRssFeedService
    {
        List<ParsedEpisodeInfo> GetRssFeed(string url);
    }
}
