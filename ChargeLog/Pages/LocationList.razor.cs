using ChargeLog.Models;
using Microsoft.AspNetCore.Components;

namespace ChargeLog.Pages
{
    public partial class LocationList : IDisposable
    {
        [Parameter]
        public int DashboardIdx { get; set; }

        [Parameter]
        public int NetworkId { get; set; }
       
        [Parameter]
        public int MonthOffset { get; set; }

        private List<LocationListItem> _locationList = new List<LocationListItem>();
        private LevelState _networkState = new();
       
        protected override async Task OnInitializedAsync()
        {
            _networkState = AppState.DashboardState.OpenItems![DashboardIdx];
            State = _networkState.OpenItems![NetworkId];
            State.OnChange += ReloadLocationList;
            _locationList = await ChargeLogService.GetLocationListAsync(NetworkId, MonthOffset);
            Config = ConfigService.GetInterfaceConfig();            
        }

        private async void ReloadLocationList()
        {
            _locationList = await ChargeLogService.GetLocationListAsync(NetworkId, MonthOffset);
            StateHasChanged();
        }

        public void Dispose()
        {
            State.OnChange -= ReloadLocationList;
        }
    }
}
