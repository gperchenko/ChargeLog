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
        List<SessionGroupListItem> GetSessionsByGroupList();
        List<KeyValue> GetGroupsBySession();
        void AddGroupToSession();
        void RemoveGroupFromSession();
        List<KeyValue> GetAllGroups();
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