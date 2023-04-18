using ChargeLog.Models;

namespace ChargeLog.Services
{
    public class ImportService : IImportService
    {
        private readonly Dictionary<string, Func<string, string, int, int, ImportResults>> _importProviders;    
        
        public ImportService() 
        {
            _importProviders = new();
            _importProviders["Test"] = new Func<string, string, int, int, ImportResults>((i, f, c, g) => { return LoadNetworks(i, f, c, g); });

        }

        public List<string> GetFileNames()
        {
            return new List<string>() { "File1", "File2" };
        }

        public List<ImportListItem> GetImports()
        {
            return new List<ImportListItem>
            {
                new ImportListItem()
                {
                    CreateDate = DateTime.Now,
                    Type = "Import1",
                    FileName = "file1",
                    NetworkCount = 1,
                    LocationCount = 2,
                    SessionCount = 3
                },
                new ImportListItem()
                {
                    CreateDate = DateTime.Now.AddDays(-1),
                    Type = "Import2",
                    FileName = "file2",
                    NetworkCount = 0,
                    LocationCount = 5,
                    SessionCount = 10
                },
            };
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

        public ImportResults ImportFile(string ImportType, string FileName, int CarId = 0, int GroupId = 0)
        {
            var provider = _importProviders[ImportType];


            return provider(ImportType, FileName, CarId, GroupId);
        }

        private ImportResults LoadNetworks(string ImportType, string FileName, int CarId = 0, int GroupId = 0)
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
