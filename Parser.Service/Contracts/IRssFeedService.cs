using Parser.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parser.Contracts
{
    public interface IRssFeedService
    {
        Task<List<ParsedEpisodeInfo>> ParseRssFeedAsync(string url);
    }
}
