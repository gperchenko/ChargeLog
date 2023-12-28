using ChargeLog.Models;

namespace ChargeLog.Services
{
    public interface IImportService
    {
        List<string> GetImportTypes();
        Task<List<ImportListItem>> GetImportsAsync();
        Task<ImportResults> ImportFileAsync(string ImportType);
    }
}
