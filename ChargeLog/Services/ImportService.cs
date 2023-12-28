using ChargeLog.Context;
using ChargeLog.DBModels;
using ChargeLog.ExtentionMethodes;
using ChargeLog.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace ChargeLog.Services
{
    public class ImportService : IImportService
    {
        private readonly Dictionary<string, Func<DateTime, int, Task<ImportResults>>> _importProviders;
        private readonly IConfigService _configService;
        private readonly ChargeLogContext _chargeLogContext;
        private readonly BackendConfig _backendConfig;

        public ImportService(IConfigService configService, ChargeLogContext context) 
        {
            _configService = configService;
            _chargeLogContext = context;
            _importProviders = new();

            _backendConfig = _configService.GetBackendConfig();

            string importType;

            importType = "Networks";
            _importProviders[importType] = new 
                Func<DateTime, int, Task<ImportResults>>(async (createDate, importId) => 
                        { return await ImportNetworksAsync(createDate, importId, importType); });

            //_importProviders["Home Sessions"] = new
            //   Func<DateTime, int, Task<ImportResults>>(async (createDate, importId) =>
            //   { return await ImportHomeChargingAsync(createDate, importId); });

            //_importProviders["ChargePoint Sessions"] = new
            //  Func<DateTime, int, Task<ImportResults>>(async (createDate, importId) =>
            //  { return await ImportChargePointAsync(createDate, importId); });

            //_importProviders["Electrify America Sessions"] = new
            //  Func<DateTime, int, Task<ImportResults>>(async (createDate, importId) =>
            //  { return await ImportElectrifyAmericaAsync(createDate, importId); });

           

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async Task<List<ImportListItem>> GetImportsAsync()
        {
            var imports = await _chargeLogContext.Imports
                 .OrderByDescending(i => i.CreateDate)
                 .Select(i => new ImportListItem()
                 {
                     CreateDate = i.CreateDate,
                     Type = i.Type,
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

        public async Task<ImportResults> ImportFileAsync(string ImportType)
        {
            var currentDate = DateTime.Now;

            var newImport = new Import()
            {
                CreateDate = currentDate,
                Type = ImportType
            };

            _chargeLogContext.Imports.Add(newImport);
            await _chargeLogContext.SaveChangesAsync();

            var provider = _importProviders[ImportType];

            var importResult = await provider(currentDate, newImport.Id);

            newImport.NetworkCount = importResult.NetworkCount;
            newImport.LocationCount = importResult.LocationCount;
            newImport.SessionCount = importResult.SessionCount;

            await _chargeLogContext.SaveChangesAsync();

            return importResult;
        }

        private async Task<ImportResults> ImportNetworksAsync(
            DateTime currentDate, 
            int importId,
            string importType)
        {
            var result = new ImportResults();

            var file = new FileInfo(_backendConfig.ImportFile!);
            var networks = await _chargeLogContext.Networks.ToListAsync();
            using var package = new ExcelPackage(file);
            await package.LoadAsync(file);
            var ws = package.Workbook.Worksheets[0];

            int row = 2;
            int col = 2;

            while (!string.IsNullOrWhiteSpace(ws.Cells[row, col].Value?.ToString()))
            {
                var newNetwork = new Network()
                {
                    Name = ws.Cells[row, col].Value.ToString(),
                    CreateDate = currentDate,
                    Rate = decimal.Parse(ws.Cells[row, col + 1].Value.ToString()!),
                    HaveAccount = bool.Parse(ws.Cells[row, col + 2].Value.ToString()!),
                    IsPartner = bool.Parse(ws.Cells[row, col + 3].Value.ToString()!),
                    DefaultChargeType = Enum.Parse<ChargeType>(ws.Cells[row, col + 4].Value.ToString()!),
                    ImportId = importId
                };
               
                var testNetwork = networks.FirstOrDefault(x => x.Name == newNetwork.Name);
                if (testNetwork == null) 
                {
                    await _chargeLogContext.Networks.AddAsync(newNetwork);
                    result.NetworkCount++;
                }
                row += 1;
            }

            await _chargeLogContext.SaveChangesAsync();

            return result;
        }

        private async Task<ImportResults> ImportHomeChargingAsync(
            DateTime currentDate,
            int importId)
        {
            var result = new ImportResults();
            var file = new FileInfo(_backendConfig.ImportFile!);
            var HomeNetwork = await _chargeLogContext.Networks
                .Include(n => n.Locations)
                .ThenInclude(l => l.Sessions)
                .AsTracking()
                .FirstOrDefaultAsync(n => n.Name == "Home");
           
            if (HomeNetwork != null && !HomeNetwork.Locations.Any())
            {
                HomeNetwork.Locations.Add(new Location() 
                { 
                    CreateDate = currentDate,
                    Name = "Home location",
                    Address = "10 Ascot Park",
                    ImportId = importId,
                    Sessions = new List<Session>()
                });

                result.LocationCount++;
               
            }

            using var package = new ExcelPackage(file);
            await package.LoadAsync(file);
            var ws = package.Workbook.Worksheets[0];

            int row = 1;
            int col = 1;

            var currentSessions = HomeNetwork?.Locations.FirstOrDefault()?.Sessions;
            var newSessions = new List<Session>();

            while (!string.IsNullOrWhiteSpace(ws.Cells[row, col].Value?.ToString()))
            {
                var date = ws.Cells[row, col].Value?.ToString();
                if (!string.IsNullOrWhiteSpace(date))
                {
                    date = date.Substring(0, date.IndexOf(" "));
                }

                if (currentSessions != null && currentSessions.Any(x => x.Date.ToShortDateString() == date))
                {
                    row ++;
                    continue;
                }

                var dr = ws.Cells[row, 6].Value.ToString();

                var kwh = decimal.Parse(ws.Cells[row, col + 3].Value.ToString()!);

                var newSsession = new Session()
                {
                    Date = DateTime.Parse(date ?? DateTime.MinValue.ToString()),
                    Duration = TimeSpan.Parse(dr),
                    KWh = kwh,
                    Price = kwh * HomeNetwork.Rate,
                    Discount = 0,
                    ChargeType = HomeNetwork.DefaultChargeType,
                 // todo   CarId = carId,
                    ImportId = importId

                };

                var foundSession = newSessions.FirstOrDefault(x => x.Date == newSsession.Date);

                if (foundSession != null)
                {
                    foundSession.Duration += newSsession.Duration;
                    foundSession.KWh += newSsession.KWh;
                    foundSession.Price += newSsession.Price;
                }
                else
                {
                    newSessions.Add(newSsession);
                }

                row ++;
            }

            currentSessions.AddRange(newSessions);
            result.SessionCount = newSessions.Count;

            await _chargeLogContext.SaveChangesAsync();

            return result;
        }

        private async Task<ImportResults> ImportChargePointAsync(
           DateTime currentDate,
           int importId)
        {
            var result = new ImportResults();
            var file = new FileInfo(_backendConfig.ImportFile!);
            var ChargePointNetwork = await _chargeLogContext.Networks
                .Include(n => n.Locations)
                .ThenInclude(l => l.Sessions)
                .AsTracking()
                .FirstOrDefaultAsync(n => n.Name == "ChargePoint");
            var existingLocations = new List<Location>();
            var newLocations = new List<Location>();

            using var package = new ExcelPackage(file);
            await package.LoadAsync(file);
            var ws = package.Workbook.Worksheets[0];

            int row = 2;
          
            while (!string.IsNullOrWhiteSpace(ws.Cells[row, 1].Value?.ToString()))
            {
                var locationName = ws.Cells[row, 5].Value?.ToString();
                var date = ws.Cells[row, 1].Value?.ToString();
                if (!string.IsNullOrWhiteSpace(date))
                {
                    date = date.Substring(0, date.IndexOf(","));
                }

                // check if this is an existing session

                var loc = ChargePointNetwork.Locations.FirstOrDefault(x => x.Name == locationName);

                if (loc != null && loc.Sessions.Any(x => x.Date.ToShortDateString() == date))
                {
                    row++;
                    continue;
                }

                var payment = ws.Cells[row, 7].Value?.ToString()!;
                var duration = ws.Cells[row, 8].Value?.ToString()!;
                var kwh = decimal.Parse(ws.Cells[row, 9].Value.ToString()!);
                var price = decimal.Parse(ws.Cells[row, 10].Value.ToString()!);

                var newSsession = new Session()
                {
                    Date = DateTime.Parse(date ?? DateTime.MinValue.ToString()),
                    Duration = TimeSpan.Parse($"{duration.GetHour()}:{duration.GetMinutes()}"),
                    KWh = kwh,
                    Price = payment == "Free" ? kwh * ChargePointNetwork.Rate : price,
                    Discount = payment == "Free" ? kwh * ChargePointNetwork.Rate : 0,
                    ChargeType = ChargePointNetwork.DefaultChargeType,
               //     CarId = carId,
                    ImportId = importId
                };

                if (loc != null)
                {
                    var loc1 = existingLocations.FirstOrDefault( x => x.Name == loc.Name);
                    if (loc1 != null) 
                    {
                        var foundSession = loc1.Sessions.FirstOrDefault(x => x.Date == newSsession.Date);

                        if (foundSession != null)
                        {
                            foundSession.Duration += newSsession.Duration;
                            foundSession.KWh += newSsession.KWh;
                            foundSession.Price += newSsession.Price;
                            foundSession.Discount += newSsession.Discount;
                        }
                        else
                        {
                            loc1.Sessions.Add(newSsession);
                            result.SessionCount++;
                        }
                    }
                    else
                    {
                        var copyLocation = new Location()
                        {
                            Name = loc.Name,
                            Sessions = new List<Session>()
                        };

                        copyLocation.Sessions.Add(newSsession);

                        existingLocations.Add(copyLocation);
                        result.SessionCount++;
                    }
                }
                else 
                {
                    var loc1 = newLocations.FirstOrDefault(x => x.Name == locationName);
                    if (loc1 != null)
                    {
                        var foundSession = loc1.Sessions.FirstOrDefault(x => x.Date == newSsession.Date);

                        if (foundSession != null)
                        {
                            foundSession.Duration += newSsession.Duration;
                            foundSession.KWh += newSsession.KWh;
                            foundSession.Price += newSsession.Price;
                            foundSession.Discount += newSsession.Discount;
                        }
                        else
                        {
                            loc1.Sessions.Add(newSsession);
                            result.SessionCount++;
                        }
                    }
                    else
                    {
                        var address = ws.Cells[row, 3].Value?.ToString()!;
                        var city = ws.Cells[row, 4].Value?.ToString()!;

                        var newLocation = new Location()
                        {
                            CreateDate = currentDate,
                            Name = locationName,
                            Address = $"{address}, {city}",
                            ImportId = importId,
                            Sessions = new List<Session>()
                        };

                        newLocation.Sessions.Add(newSsession);

                        newLocations.Add(newLocation);
                        result.SessionCount++;
                        result.LocationCount++;
                    }

                }

                row++;
            }

            foreach (var loc in existingLocations)
            {
                var realLocation = ChargePointNetwork.Locations.FirstOrDefault( l => l.Name == loc.Name);

                if (realLocation != null)
                {
                    realLocation.Sessions.AddRange(loc.Sessions);
                }
            }

            ChargePointNetwork.Locations.AddRange(newLocations);

            await _chargeLogContext.SaveChangesAsync();

            return result;
        }

        private async Task<ImportResults> ImportElectrifyAmericaAsync(
           DateTime currentDate,
           int importId)
        {
            var result = new ImportResults();
            var file = new FileInfo(_backendConfig.ImportFile!);
            var ElectrifyAmericaNetwork = await _chargeLogContext.Networks
                .Include(n => n.Locations)
                .ThenInclude(l => l.Sessions)
                .AsTracking()
                .FirstOrDefaultAsync(n => n.Name == "Electrify America");
            var existingLocations = new List<Location>();
            var newLocations = new List<Location>();

            using var package = new ExcelPackage(file);
            await package.LoadAsync(file);
            var ws = package.Workbook.Worksheets[0];

            int row = 2;

            while (!string.IsNullOrWhiteSpace(ws.Cells[row, 1].Value?.ToString()))
            {
                var locationName = ws.Cells[row, 2].Value?.ToString();
                var date = ws.Cells[row, 1].Value?.ToString();
                if (!string.IsNullOrWhiteSpace(date))
                {
                    date = date.Substring(0, date.IndexOf(" "));
                }

                // check if this is an existing session

                var loc = ElectrifyAmericaNetwork.Locations.FirstOrDefault(x => x.Name == locationName);

                if (loc != null && loc.Sessions.Any(x => x.Date.ToShortDateString() == date))
                {
                    row++;
                    continue;
                }

                var totalCost = decimal.Parse(ws.Cells[row, 4].Value.ToString()!);
                var basicCharge = decimal.Parse(ws.Cells[row, 5].Value.ToString()!);
                var discount = ws.Cells[row, 6].Value?.ToString()!;
                var kwh = decimal.Parse(ws.Cells[row, 7].Value.ToString()!);

            //    var newSsession = new Session()
            //    {
            //        Date = DateTime.Parse(date ?? DateTime.MinValue.ToString()),
            //        Duration = TimeSpan.Parse($"{duration.GetHour()}:{duration.GetMinutes()}"),
            //        KWh = kwh,
            //        Price = payment == "Free" ? kwh * ChargePointNetwork.Rate : price,
            //        Discount = payment == "Free" ? kwh * ChargePointNetwork.Rate : 0,
            //        ChargeType = ChargePointNetwork.DefaultChargeType,
            //        CarId = carId,
            //        ImportId = importId
            //    };

            //    if (loc != null)
            //    {
            //        var loc1 = existingLocations.FirstOrDefault(x => x.Name == loc.Name);
            //        if (loc1 != null)
            //        {
            //            var foundSession = loc1.Sessions.FirstOrDefault(x => x.Date == newSsession.Date);

            //            if (foundSession != null)
            //            {
            //                foundSession.Duration += newSsession.Duration;
            //                foundSession.KWh += newSsession.KWh;
            //                foundSession.Price += newSsession.Price;
            //                foundSession.Discount += newSsession.Discount;
            //            }
            //            else
            //            {
            //                loc1.Sessions.Add(newSsession);
            //                result.SessionCount++;
            //            }
            //        }
            //        else
            //        {
            //            var copyLocation = new Location()
            //            {
            //                Name = loc.Name,
            //                Sessions = new List<Session>()
            //            };

            //            copyLocation.Sessions.Add(newSsession);

            //            existingLocations.Add(copyLocation);
            //            result.SessionCount++;
            //        }
            //    }
            //    else
            //    {
            //        var loc1 = newLocations.FirstOrDefault(x => x.Name == locationName);
            //        if (loc1 != null)
            //        {
            //            var foundSession = loc1.Sessions.FirstOrDefault(x => x.Date == newSsession.Date);

            //            if (foundSession != null)
            //            {
            //                foundSession.Duration += newSsession.Duration;
            //                foundSession.KWh += newSsession.KWh;
            //                foundSession.Price += newSsession.Price;
            //                foundSession.Discount += newSsession.Discount;
            //            }
            //            else
            //            {
            //                loc1.Sessions.Add(newSsession);
            //                result.SessionCount++;
            //            }
            //        }
            //        else
            //        {
            //            var address = ws.Cells[row, 3].Value?.ToString()!;
            //            var city = ws.Cells[row, 4].Value?.ToString()!;

            //            var newLocation = new Location()
            //            {
            //                CreateDate = currentDate,
            //                Name = locationName,
            //                Address = $"{address}, {city}",
            //                ImportId = importId,
            //                Sessions = new List<Session>()
            //            };

            //            newLocation.Sessions.Add(newSsession);

            //            newLocations.Add(newLocation);
            //            result.SessionCount++;
            //            result.LocationCount++;
            //        }

            //    }

            //    row++;
            }

            //foreach (var loc in existingLocations)
            //{
            //    var realLocation = ChargePointNetwork.Locations.FirstOrDefault(l => l.Name == loc.Name);

            //    if (realLocation != null)
            //    {
            //        realLocation.Sessions.AddRange(loc.Sessions);
            //    }
            //}

            //ChargePointNetwork.Locations.AddRange(newLocations);

            //await _chargeLogContext.SaveChangesAsync();

            return result;
        }
    }
}
