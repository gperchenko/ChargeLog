using ChargeLog.Context;
using ChargeLog.DBModels;
using ChargeLog.Models;
using ChargeLog.ExtentionMethodes;
using Microsoft.EntityFrameworkCore;

namespace ChargeLog.Services
{
    public class ChargeLogService : IChargeLogService
    {
        private readonly IConfiguration? _config;
        private readonly  ChargeLogContext _chargeLogContext;

        public ChargeLogService(IConfiguration config, ChargeLogContext context)
        {
            _config = config;
            _chargeLogContext = context;
        }

        public InterfaceConfig GetConfig()
        {
            var interfaceConfig = new InterfaceConfig();
            _config?.GetSection(InterfaceConfig.InterfaceParams).Bind(interfaceConfig);

            return interfaceConfig;
        }

        public async Task<DashboardMainTableRow> GetTotalsAsync(int monthOffset = 1)
        {
            var networks = await _chargeLogContext.Networks
                .Include(n => n.Locations)
                .ThenInclude(l => l.Sessions).ToListAsync();

            var filteredNetworks = networks.FilterNetworks(monthOffset).ToList();

            var currentDate = DateTime.Now;
            currentDate = currentDate.AddMonths(monthOffset);

            var locationList = filteredNetworks.SelectMany(n => n.Locations).ToList();
            var sessionList = locationList.SelectMany(l => l.Sessions).ToList();

            var result = new DashboardMainTableRow()
            {
                Date = currentDate,
                NetworkCount = filteredNetworks.Count,
                LocationCount = locationList.Count,
                MonthOffset= monthOffset,
                SessionCount = sessionList.Count,
                KWh = sessionList.Sum(s => s.KWh),
                Duration = sessionList.DurationSum(),
                Price = sessionList.Sum(s => s.Price),
                Discount = sessionList.Sum(s => s.Discount),
            };

            return result;
        }

        public async Task<List<NetworkListItem>> GetNetworkListAsync(int monthOffset)
        {
           var networks = await _chargeLogContext.Networks
                .Include(n => n.Locations)
                .ThenInclude(l => l.Sessions).ToListAsync();

            var networkList = networks.FilterNetworks(monthOffset).Select(n => new NetworkListItem()
            {
                Id = n.Id,
                Name = n.Name,
                LocationCount = n.Locations.Count,
                SessionCount = n.Locations.SelectMany(l => l.Sessions).Count(),
                KWh = n.Locations.SelectMany(l => l.Sessions).Sum(s => s.KWh),
                Duration = n.Locations.SelectMany(l => l.Sessions).DurationSum(),
                Price = n.Locations.SelectMany(l => l.Sessions).Sum(s => s.Price),
                Discount = n.Locations.SelectMany(l => l.Sessions).Sum(s => s.Discount),
            }).ToList();

            return networkList;            
        }

        public async Task<List<LocationListItem>> GetLocationListAsync(int networkId, int monthOffset)
        {
            var locations = await _chargeLogContext.Locations
                .Where (l => l.NetworkId == networkId)
                .Include(l => l.Sessions).ToListAsync();

            var locationList = locations.FilterLocations(monthOffset).Select(l => new LocationListItem()
            {
                Name = l.Name,
                Id = l.Id,
                Address = l.Address,
                SessionCount = l.Sessions.Count,
                KWh = l.Sessions.Sum(s => s.KWh),
                Duration = l.Sessions.DurationSum(),
                Price = l.Sessions.Sum(s => s.Price),
                Discount = l.Sessions.Sum(s => s.Discount),
            }).ToList();

            return locationList;
        }

        public async Task<List<SessionListItem>> GetSessionListAsync(int locationId, int monthOffset)
        {
            var sessions = await _chargeLogContext.Sessions                
                .Where(s => s.LocationId == locationId)
                .Include(s => s.Car)
                .Include(s => s.ThroughNetwork)
                .ToListAsync();

            var sessionList = sessions.FilterSessions(monthOffset).Select(s => new SessionListItem()
            {
                Id = s.Id,
                Date = s.Date,
                Duration = s.Duration,
                KWh = s.KWh,
                Price = s.Price,
                Discount = s.Discount,
                ChargeType = s.ChargeType.ToString(),
                Car = s.Car?.DisplayName,
                ThroughNetwork = s.ThroughNetwork?.Name
            }).ToList();

             return sessionList.OrderByDescending(s => s.Date).ToList();           
        }

        public Task<List<Car>> GetCarsAsync()
        {
            return _chargeLogContext.Cars.ToListAsync();
        }

        public async Task AddCarAsync(Car car)
        {
            await _chargeLogContext.Cars.AddAsync(car);
            await _chargeLogContext.SaveChangesAsync();
        }

        public async Task<List<GroupListItem>> GetGroupsTotalsAsync()
        {
            var groups = await _chargeLogContext.Groups
                .Include(g => g.Sessions)
                .ThenInclude(s => s.Location)
                .ThenInclude(l => l.Network)
                .Include(g => g.Sessions)
                .ThenInclude(s => s.Car)
                .Include(g => g.Sessions)
                .ThenInclude(s => s.ThroughNetwork)
                .ToListAsync();

            var groupItemList = groups.Select(g => new GroupListItem() 
                    {
                        Id = g.Id,
                        Name = g.Name,
                        NetworkCount = g.Sessions.Select(s => s.Location?.Network?.Name).Distinct().Count(),
                        LocationCount = g.Sessions.Select(s => s.Location?.Name).Distinct().Count(),
                        SessionCount = g.Sessions.Count(),
                        KWh = g.Sessions.Sum( s => s.KWh),
                        Duration = g.Sessions.DurationSum(),
                        Price = g.Sessions.Sum(s => s.Price),
                        Discount = g.Sessions.Sum(s => s.Discount)
                    }).ToList();

            return groupItemList;
        }
        
        public async Task<List<SessionGroupListItem>> GetSessionsByGroupAsync(int groupId)
        {
            var group = await _chargeLogContext.Groups
                .Include(g => g.Sessions)
                .ThenInclude(s => s.Location)
                .ThenInclude(l => l.Network)
                .Include(g => g.Sessions)
                .ThenInclude(s => s.Car)
                .Include(g => g.Sessions)
                .ThenInclude(s => s.ThroughNetwork)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            var result = group.Sessions
                .Select(s => new SessionGroupListItem()
                {
                    Network = s.Location?.Network?.Name,
                    Location = s.Location?.Name,
                    Id = s.Id,
                    Date= s.Date,
                    Duration= s.Duration,
                    KWh = s.KWh,
                    Price = s.Price,
                    Discount = s.Discount,
                    ChargeType = s.ChargeType.ToString(),
                    Car = s.Car?.DisplayName,
                    ThroughNetwork = s.ThroughNetwork?.Name

                }).ToList();

            return result;
        }

        public async Task AddGroupAsync(Group group)
        {
            await _chargeLogContext.Groups.AddAsync(group);
            await _chargeLogContext.SaveChangesAsync();
        }

        public async Task AddNetworkAsync(Network network)
        {
            await _chargeLogContext.Networks.AddAsync(network);
            await _chargeLogContext.SaveChangesAsync();
        }

        public async Task AddLocationAsync(Location location)
        {
            await _chargeLogContext.Locations.AddAsync(location);
            await _chargeLogContext.SaveChangesAsync();
        }

        public async Task AddSessioonAsync(Session session)
        {
            await _chargeLogContext.Sessions.AddAsync(session);
            await _chargeLogContext.SaveChangesAsync();
        }

        public Task<Network?> GetNetworkAsync(int networkId)
        {            
            return _chargeLogContext.Networks.FirstOrDefaultAsync(n => n.Id == networkId); ;
        }

        public Task<List<Network>> GetNetworksPartnerWithAccount()
        {
            return _chargeLogContext.Networks.Where(n => n.IsPartner && n.HaveAccount).ToListAsync();
        }

        public async Task<List<KeyValue>> GetGroupsBySessionAsync(int sessionId)
        {
            var session = await _chargeLogContext.Sessions
                .Include(s => s.Groups)
                .FirstOrDefaultAsync(s => s.Id == sessionId);
            if (session == null || session.Groups == null) return new List<KeyValue>();

            var result = session.Groups.Select(g => new KeyValue() { Key = g.Id, Value = g.Name}).ToList();
            return result;
        }

        public async Task AddGroupToSessionAsync(int sessionId, int groupId)
        {
            var session = await _chargeLogContext.Sessions
                .Include(s => s.Groups)
                .AsTracking()
                .FirstOrDefaultAsync(s => s.Id == sessionId);
            if (session == null) return;

            var group = _chargeLogContext.Groups.FirstOrDefault(g => g.Id == groupId);
            if (group == null) return;

            if (session.Groups == null)
                session.Groups = new List<Group>();

            session.Groups.Add(group);
            _chargeLogContext.SaveChanges();

        }

        public async Task RemoveGroupFromSessionAsync(int sessionId, int groupId )
        {
            var session = await _chargeLogContext.Sessions
                .Include(s => s.Groups)
                .AsTracking()
                .FirstOrDefaultAsync(s => s.Id == sessionId);
            if (session == null || session.Groups == null) return;

            session.Groups.RemoveAll(g => g.Id == groupId);
            _chargeLogContext.SaveChanges();
        }

        public async Task<List<KeyValue>> GetAllGroupsAsync()
        {
            var groups = await _chargeLogContext.Groups.ToListAsync();
            var result = groups.Select(g => new KeyValue() { Key = g.Id, Value = g.Name}).ToList();

            return result;
        }
    }
}
