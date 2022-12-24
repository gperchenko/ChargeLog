using ChargeLog.DBModels;
using ChargeLog.Models;
using Microsoft.AspNetCore.Components;

namespace ChargeLog.Pages
{
    public partial class SessionList : IDisposable
    {
        [Parameter]
        public int DashboardIdx { get; set; }

        [Parameter]
        public int NetworkId { get; set; }

        [Parameter]
        public int LocationId { get; set; }

        private List<SessionListItem> _sessionList = new List<SessionListItem>();
        private LevelState _networkState = new();
        private LevelState _locationState = new();

        protected override async Task OnInitializedAsync()
        {
            _networkState = AppState.DashboardState.OpenItems![DashboardIdx];
            _locationState = _networkState.OpenItems![NetworkId];
            State = _locationState.OpenItems![LocationId];
            State.OnChange += ReloadSessionList;
            _sessionList = await ChargeLogService.GetSessionListAsync(LocationId);
            Config = ChargeLogService.GetConfig();
        }

        private async void ReloadSessionList()
        {
            _sessionList = await ChargeLogService.GetSessionListAsync(LocationId);
            StateHasChanged();
        }

        public void Dispose()
        {
            State.OnChange -= ReloadSessionList;
        }
    }
}
