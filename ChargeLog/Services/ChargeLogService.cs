using ChargeLog.Context;
using ChargeLog.DBModels;
using ChargeLog.Models;
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

        public Task<DashboardMainTableRow> GetTotals()
        {
            return Task.Run(() =>
                {
                    return new DashboardMainTableRow()
                    {
                        NetworkCount = 5,
                        LocationCount = 7,
                        SessionCount = 5,
                        KWh = 200.70,
                        Duration = TimeSpan.FromMinutes(2000),
                        Price = 400.00,
                        Discount = 200
                    };
                });
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

        public Task<List<NetworkListItem>> GetNetworkListAsync()
        {
            var networkList = new List<NetworkListItem>()
            {
                new NetworkListItem()
                {
                    NetworkName = "Network Name",
                    NetworkId = 1,
                    LocationCount = 7,
                    SessionCount = 5,
                    KWh = 200.70,
                    Duration = TimeSpan.FromMinutes(2000),
                    Price = 400.00,
                    Discount = 200
                },
                new NetworkListItem()
                {
                    NetworkName = "Network Name 2",
                    NetworkId = 2,
                    LocationCount = 7,
                    SessionCount = 5,
                    KWh = 200.70,
                    Duration = TimeSpan.FromMinutes(2000),
                    Price = 400.00,
                    Discount = 200
                }


            };


            return Task.Run(() =>
            {
                return networkList;
            });
        }

        public Task<List<LocationListItem>> GetLocationListAsync()
        {
            var locationList = new List<LocationListItem>()
            {
                new LocationListItem()
                {
                    LocationName = "Location Name",
                    LocationId = 1,
                    LocationAddress = "Location Address",
                    SessionCount = 5,
                    KWh = 200.70,
                    Duration = TimeSpan.FromMinutes(2000),
                    Price = 400.00,
                    Discount = 200
                },
                new LocationListItem()
                {
                    LocationName = "Network Name 2",
                    LocationId = 2,
                    LocationAddress = "Location Address 2",
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
