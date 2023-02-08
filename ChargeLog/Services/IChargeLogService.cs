using ChargeLog.Models;
using ChargeLog.DBModels;

namespace ChargeLog.Services
{
    public interface IChargeLogService
    {
        Task<DashboardMainTableRow> GetTotalsAsync(int monthOffset=1);
        Task<List<NetworkListItem>> GetNetworkListAsync(int monthOffset);
        Task<List<LocationListItem>> GetLocationListAsync(int networkId, int monthOffset);
        Task<List<SessionListItem>> GetSessionListAsync(int locationId, int monthOffset);
        Task<List<SessionGroupListItem>> GetSessionsByGroupAsync(int groupId);
        Task<List<KeyValue>> GetGroupsBySessionAsync(int sessionId);
        Task AddGroupToSessionAsync(int sessionId, int groupId);
        Task RemoveGroupFromSessionAsync(int sessionId, int groupId);
        Task<List<KeyValue>> GetAllGroupsAsync();
        InterfaceConfig GetConfig();
        Task<List<Car>> GetCarsAsync();
        Task AddCarAsync(Car car);
        Task<List<GroupListItem>> GetGroupsTotalsAsync();
        Task AddGroupAsync(Group group);
        Task AddNetworkAsync(Network network);
        Task AddLocationAsync(Location location);
        Task AddSessioonAsync(Session session);
        Task<Network?> GetNetworkAsync(int networkId);
        Task<List<Network>> GetNetworksPartnerWithAccount();
    }
}