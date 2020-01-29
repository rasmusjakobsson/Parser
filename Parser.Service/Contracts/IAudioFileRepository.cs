using System.Threading.Tasks;

namespace Parser.Contracts
{
    public interface IAudioFileRepository
    {
        Task<string> CalculateFileCheckSumAsync(string fileUrl);
    }
}
