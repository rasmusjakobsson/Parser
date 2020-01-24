using System.ServiceModel.Syndication;

namespace Parser.Contracts
{
    public interface IRssFeedRepository
    {
        SyndicationFeed CallRssFeed(string url);
    }
}
