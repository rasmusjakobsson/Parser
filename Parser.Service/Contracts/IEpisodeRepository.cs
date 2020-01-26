using Parser.Models;

namespace Parser.Contracts
{
    public interface IShowRepository
    {
        ShowResponse GetShowInfo(string showId);
    }
}
