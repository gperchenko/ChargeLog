using ChargeLog.Context;
using ChargeLog.DBModels;
using ChargeLog.Models;
using ChargeLog.Pages;
using Microsoft.EntityFrameworkCore;
using System;

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

        public async Task<DashboardMainTableRow> GetTotalsAsync()
        {
            var networks = await _chargeLogContext.Networks
                .Include(n => n.Locations)
                .ThenInclude(l => l.Sessions).ToListAsync();

            var locationList = networks.SelectMany(n => n.Locations).ToList();
            var sessionList = locationList.SelectMany(l => l.Sessions).ToList();

            var result = new DashboardMainTableRow()
            {
                NetworkCount = networks.Count,
                LocationCount = locationList.Count,
                SessionCount = sessionList.Count,
                KWh = sessionList.Sum(s => s.KWh),
                Duration = TimeSpan.FromMinutes(0),
                Price = sessionList.Sum(s => s.Price),
                Discount = sessionList.Sum(s => s.Discount),

            };

            return result;
        }

        public DashboardMainTableRow GetMonth(int monthOffset)
        {
            var currentDate = DateTime.Now;
            currentDate = currentDate.AddMonths(monthOffset);

            return new DashboardMainTableRow()
            {
                Date = currentDate,
                NetworkCount = 3,
                LocationCount = 4,
                SessionCount = 5,
                KWh = 50.35,
                Duration = TimeSpan.FromMinutes(200),
                Price = 60.00,
                Discount = 30
            };
        }

        public async Task<List<NetworkListItem>> GetNetworkListAsync()
        {
            var networkList = new List<NetworkListItem>();

            var networks = await _chargeLogContext.Networks
                .Include(n => n.Locations)
                .ThenInclude(l => l.Sessions).ToListAsync();

            foreach(var network in networks)
            {
                var netwoorkIten = new NetworkListItem()
                {
                    Id = network.Id,
                    Name = network.Name,
                    LocationCount = network.Locations.Count,
                    SessionCount = 0,
                    KWh = 0,
                    Duration = TimeSpan.FromMinutes(0),
                    Price = 0,
                    Discount = 0
                };

                networkList.Add(netwoorkIten);
            }

            return networkList;
            
        }

        public async Task<List<LocationListItem>> GetLocationListAsync(int networkId)
        {
            var locationList = new List<LocationListItem>();

            var locations = await _chargeLogContext.Locations
                .Where (l => l.NetworkId == networkId)
                .Include(l => l.Sessions).ToListAsync();

            foreach(var location in locations)
            {
                var locationItem = new LocationListItem()
                {
                    Name = location.Name,
                    Id = location.Id,
                    Address = location.Address,
                    SessionCount = location.Sessions.Count,
                    KWh = 0,
                    Duration = TimeSpan.FromMinutes(0),
                    Price = 0,
                    Discount = 0
                };

                locationList.Add(locationItem);
            }

            return locationList;
        }

        public async Task<List<SessionListItem>> GetSessionListAsync(int locationId)
        {
            var sessionList = new List<SessionListItem>();

            var sessions = await _chargeLogContext.Sessions
                .Where(s => s.LocationId == locationId).ToListAsync();

            foreach(var session in sessions)
            {
                var SessionListItem = new SessionListItem()
                {
                    Id = session.Id,
                    Date = session.Date,
                    Duration = session.Duration,
                    KWh = session.KWh,
                    Price = session.Price,
                    Discount = session.Discount,
                    ChargeType = session.ChargeType.ToString(),
                    Car = "Kia",
                    ThroughNetwork = ""
                };

                sessionList.Add(SessionListItem);
            }

             return sessionList;           
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
            var groupItemList = new List<GroupListItem>();

            var groups = await _chargeLogContext.Groups.ToListAsync();

            foreach (var group in groups)
            {
                var GroupListItem = new GroupListItem()
                {
                    Id= group.Id,
                    Name= group.Name,
                    NetworkCount = 0,
                    LocationCount = 0,
                    SessionCount = 0,
                    KWh = 0,
                    Duration = TimeSpan.FromMinutes(0),
                    Price = 0,
                    Discount = 0
                };

                groupItemList.Add(GroupListItem);
            }

            return groupItemList;
        }
        
        public List<SessionGroupListItem> GetSessionsByGroupList()
        {
            return new List<SessionGroupListItem>()
                { new SessionGroupListItem()
                    {
                        Network = "Net1",
                        Location = "Loc1",
                        ChargeType = ChargeType.DC.ToString(),
                        Car = "Kia"
                    },
                    new SessionGroupListItem()
                    {
                        Network = "Net2",
                        Location = "Loc2",
                        ChargeType = ChargeType.AC.ToString(),
                        Car = "Kia"
                    }
                };
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

        public List<KeyValue> GetGroupsBySession()
        {
            return new List<KeyValue>
            {
                new KeyValue() {Key = 1, Value = "Group 1" }
            };
        }

        public void AddGroupToSession()
        {
            
        }

        public void RemoveGroupFromSession()
        {
           
        }

        public List<KeyValue> GetAllGroups()
        {
            return new List<KeyValue>
            {
                new KeyValue() {Key = 1, Value = "Group 1" },
                new KeyValue() {Key = 2, Value = "Group 2" },
                new KeyValue() {Key = 3, Value = "Group 3"}
            };
        }
    }
}
