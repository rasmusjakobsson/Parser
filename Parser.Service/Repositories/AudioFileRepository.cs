using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Parser.Contracts;

namespace Parser.Repositories
{
    public class AudioFileRepository:IAudioFileRepository
    {
        public async Task<string> CalculateFileCheckSumAsync(string fileUrl)
        {
            var httpClient = new HttpClient();
            await using var stream = await httpClient.GetStreamAsync(fileUrl);
            var hash = MD5.Create().ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
