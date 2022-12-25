using ChargeLog.Models;
using ChargeLog.DBModels;

namespace ChargeLog.Services
{
    public interface IChargeLogService
    {
        Task<DashboardMainTableRow> GetTotalsAsync();
        DashboardMainTableRow GetMonth(int monthOffset);
        Task<List<NetworkListItem>> GetNetworkListAsync();
        Task<List<LocationListItem>> GetLocationListAsync(int networkId);
        Task<List<SessionListItem>> GetSessionListAsync(int locationId);
        InterfaceConfig GetConfig();
        Task<List<Car>> GetCarsAsync();
        Task AddCarAsync(Car car);
        Task<List<GroupListItem>> GetGroupsAsync();
        Task AddGroupAsync(Group group);
        Task AddNetworkAsync(Network network);
        Task AddLocationAsync(Location location);
        Task AddSessioonAsync(Session session);
        Task<Network?> GetNetworkAsync(int networkId);
        Task<List<Network>> GetNetworksPartnerWithAccount();
    }
}