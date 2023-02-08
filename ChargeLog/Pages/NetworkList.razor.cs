using ChargeLog.Models;
using Microsoft.AspNetCore.Components;

namespace ChargeLog.Pages
{
    public partial class NetworkList : IDisposable
    {
        [Parameter]
        public int DashboardIdx { get; set; }
        [Parameter]
        public int MonthOffset { get; set; }

        private List<NetworkListItem> _networkList = new();

        protected override async Task OnInitializedAsync()
        {
            State = AppState.DashboardState.OpenItems![DashboardIdx];
            State.OnChange += ReloadNetworkList;
            Config = ChargeLogService.GetConfig();
            _networkList = await ChargeLogService.GetNetworkListAsync(MonthOffset);            
        }

        private async void ReloadNetworkList()
        {
            _networkList = await ChargeLogService.GetNetworkListAsync(MonthOffset);
            StateHasChanged();
        }

        public void Dispose()
        {
            State.OnChange -= ReloadNetworkList;
        }
    }
}
