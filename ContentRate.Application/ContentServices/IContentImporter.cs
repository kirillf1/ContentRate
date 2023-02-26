using Ardalis.Result;
using ContentRate.Application.Contracts.Content;

namespace ContentRate.Application.ContentServices
{
    public interface IContentImporter
    {
        Task<Result<IEnumerable<ContentImport>>> Import();
    }
}
