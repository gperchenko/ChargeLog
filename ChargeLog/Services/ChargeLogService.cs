using ChargeLog.Context;
using ChargeLog.DBModels;
using ChargeLog.Models;
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
                .AsNoTracking()
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
                .AsNoTracking()
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

        public Task<List<LocationListItem>> GetLocationListAsync()
        {
            var locationList = new List<LocationListItem>()
            {
                new LocationListItem()
                {
                    Name = "Location Name",
                    Id = 1,
                    Address = "Location Address",
                    SessionCount = 5,
                    KWh = 200.70,
                    Duration = TimeSpan.FromMinutes(2000),
                    Price = 400.00,
                    Discount = 200
                },
                new LocationListItem()
                {
                    Name = "Network Name 2",
                    Id = 2,
                    Address = "Location Address 2",
                    SessionCount = 5,
                    KWh = 200.70,
                    Duration = TimeSpan.FromMinutes(2000),
                    Price = 400.00,
                    Discount = 200
                }


            };


            return Task.Run(() =>
            {
                return locationList;
            });
        }

        public Task<List<SessionListItem>> GetSessionListAsync()
        {
            var sessionList = new List<SessionListItem>()
            {
                new SessionListItem()
                {
                    Date = DateTime.Now,
                    Duration = TimeSpan.Parse("01:20"),
                    KWh = 25.6,
                    Price = 10,
                    Discount = 0,
                    ChargeType = "AC",
                    Car = "Kia",
                    ThroughNetwork = ""
                },
                new SessionListItem()
                {
                    Date = DateTime.Now.AddDays(-5),
                    Duration = TimeSpan.Parse("00:30"),
                    KWh = 25.6,
                    Price = 10,
                    Discount = 10,
                    ChargeType = "AC",
                    Car = "Kia",
                    ThroughNetwork = "ChargePoint"
                }
            };

            return Task.Run(() =>
            {
                return sessionList;
            });
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

        public Task<List<Group>> GetGroupsAsync()
        {
            return _chargeLogContext.Groups.ToListAsync();
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
    }
}
