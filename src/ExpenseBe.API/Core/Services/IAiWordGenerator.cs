using System.Threading;
using System.Threading.Tasks;

namespace ExpenseBe.Core.Services
{
    public interface IAiWordGenerator
    {
        Task<string> GenerateAsync(string word, CancellationToken cancellationToken = default);
    }
}
