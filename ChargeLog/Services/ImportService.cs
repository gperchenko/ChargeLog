using ChargeLog.Models;

namespace ChargeLog.Services
{
    public class ImportService : IImportService
    {
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
            return new List<string>() { "Import1", "import2" };
        }

        public ImportResults ImportFile(string ImportType, string FileName, int CarId = 0, int GroupId = 0)
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
