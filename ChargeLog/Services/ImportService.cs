using ChargeLog.Context;
using ChargeLog.DBModels;
using ChargeLog.Models;
using Microsoft.EntityFrameworkCore;

namespace ChargeLog.Services
{
    public class ImportService : IImportService
    {
        private readonly Dictionary<string, Func<DateTime, int, string, string, int, int, ImportResults>> _importProviders;
        private readonly IConfigService _configService;
        private readonly ChargeLogContext _chargeLogContext;

        public ImportService(IConfigService configService, ChargeLogContext context) 
        {
            _configService = configService;
            _chargeLogContext = context;
            _importProviders = new();
            _importProviders["Test"] = new 
                Func<DateTime, int, string, string, int, int, ImportResults>((createDate, importId, importType, fileName, carId, groupId) => 
                        { return LoadNetworks(createDate, importId, importType, fileName, carId, groupId); });
        }

        public async Task<List<string>> GetFileNamesAsync()
        {
            var imports = await _chargeLogContext.Imports.ToListAsync();
            var backendConfig = _configService.GetBackendConfig();

            var files = Directory.GetFiles(backendConfig.ImportFolder!, backendConfig.FileSearchPatten!)
                .Select(f => f.Replace(backendConfig.ImportFolder!, ""))
                .Where(f => imports.All(i => i.FileName != f)).ToList();

            return files;
        }

        public async Task<List<ImportListItem>> GetImportsAsync()
        {
            var imports = await _chargeLogContext.Imports
                 .OrderByDescending(i => i.CreateDate)
                 .Select(i => new ImportListItem()
                 {
                     CreateDate = i.CreateDate,
                     Type = i.Type,
                     FileName = i.FileName,
                     NetworkCount = i.NetworkCount,
                     LocationCount = i.LocationCount,
                     SessionCount = i.SessionCount
                 })
                .ToListAsync();

            return imports;
        }

        public List<string> GetImportTypes()
        {
            List<string> result = new List<string>();

            foreach (var (key, type) in _importProviders)
            {
                result.Add(key);
            }

            return result;
        }

        public async Task<ImportResults> ImportFileAsync(string ImportType, string FileName, int CarId = 0, int GroupId = 0)
        {
            var currentDate = DateTime.Now;

            var newImport = new Import()
            {
                CreateDate = currentDate,
                Type = ImportType,
                FileName = FileName,
            };

            _chargeLogContext.Imports.Add(newImport);
            await _chargeLogContext.SaveChangesAsync();

            var provider = _importProviders[ImportType];

            var importResult = provider(currentDate, newImport.Id, ImportType, FileName, CarId, GroupId);

            newImport.NetworkCount = importResult.NetworkCount;
            newImport.LocationCount = importResult.LocationCount;
            newImport.SessionCount = importResult.SessionCount;

            await _chargeLogContext.SaveChangesAsync();

            return importResult;
        }

        private ImportResults LoadNetworks(
            DateTime currentDate, 
            int importId, 
            string importType, 
            string fileName, 
            int carId = 0, 
            int groupId = 0)
        {
            return new ImportResults()
            {
                NetworkCount = 0,
                LocationCount = 3,
                SessionCount = 5,
            };
        }
    }
}
