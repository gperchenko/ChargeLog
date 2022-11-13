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
