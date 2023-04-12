using ChargeLog.Models;

namespace ChargeLog.Services
{
    public interface IImportService
    {
        List<string> GetImportTypes();
        List<string> GetFileNames();
        List<ImportListItem> GetImports();
        ImportResults ImportFile(string ImportType, string FileName, int CarId = 0, int GroupId = 0);
    }
}
