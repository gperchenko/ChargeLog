using ChargeLog.Models;

namespace ChargeLog.Services
{
    public interface IImportService
    {
        List<string> GetImportTypes();
        Task<List<string>> GetFileNamesAsync();
        Task<List<ImportListItem>> GetImportsAsync();
        Task<ImportResults> ImportFileAsync(string ImportType, string FileName, int CarId = 0, int GroupId = 0);
    }
}
