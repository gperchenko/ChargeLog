using ChargeLog.Models;
using Microsoft.AspNetCore.Components;

namespace ChargeLog.Pages
{
    public partial class NetworkList
    {
        [Parameter]
        public int DashboardIdx { get; set; }

        private List<NetworkListItem> _networkList = new();

        protected override async Task OnInitializedAsync()
        {
            State = AppState.DashboardState.OpenItems[DashboardIdx];
            State.OnChange += ReloadNetworkList;
            Config = ChargeLogService.GetConfig();
            _networkList = await ChargeLogService.GetNetworkListAsync();            
        }

        private async void ReloadNetworkList()
        {
            _networkList = await ChargeLogService.GetNetworkListAsync();
            StateHasChanged();
        }

        public void Dispose()
        {
            State.OnChange -= ReloadNetworkList;
        }
    }
}
