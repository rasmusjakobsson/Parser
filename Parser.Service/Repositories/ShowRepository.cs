using Parser.Contracts;
using Parser.Models;
using RestSharp;

namespace Parser.Repositories
{
    public class ShowRepository : IShowRepository
    {
        private const string ShowApiBaseUrl = "https://feeder.acast.com";
        public ShowResponse GetShowInfo(string showId)
        {
            var client = new RestClient(ShowApiBaseUrl);
            var request = new RestRequest($"api/v1/shows/{showId}");
            var response = client.Execute<ShowResponse>(request);
            return response.Data;
        }
    }
}
