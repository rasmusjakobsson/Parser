using System.ServiceModel.Syndication;
using System.Xml;
using Parser.Contracts;

namespace Parser.Repositories
{
    public class RssFeedRepository: IRssFeedRepository
    {
        public SyndicationFeed CallRssFeed(string url)
        {
            using var reader = XmlReader.Create(url);
            return SyndicationFeed.Load(reader);
        }
    }
}
