using ChargeLog.DBModels;
using ChargeLog.ViewModels;
using Microsoft.AspNetCore.Components;

namespace ChargeLog.Pages
{
    public partial class NewLocation
    {
        [Parameter]
        public int NetworkId { get; set; }

        private LocationView newLocation = new LocationView();

        private async Task AddLocation()
        {
            var location = new Location()
            {
                Name = newLocation.Name,
                Address = newLocation.Address,
                NetworkId = NetworkId
            };

            await ChargeLogService.AddLocationAsync(location);
            AppState.DashboardState.NotifyStateChanged();

            newLocation = new LocationView();
        }
    }
}
