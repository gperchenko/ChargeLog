using ChargeLog.Models;
using ChargeLog.DBModels;

namespace ChargeLog.Services
{
    public interface IChargeLogService
    {
        Task<DashboardMainTableRow> GetTotals();
        DashboardMainTableRow GetMonth(int monthOffset);
        InterfaceConfig GetConfig();
        Task<List<Car>> GetCarsAsync();
        Task AddCarAsync(Car car);
        Task<List<Group>> GetGroupsAsync();
        Task AddGroupAsync(Group group);
        Task AddNetworkAsync(Network network);
        Task AddLocationAsync(Location location);
        Task AddSessioonAsync(Session session);
        Task<Network?> GetNetworkAsync(int networkId);
    }
}