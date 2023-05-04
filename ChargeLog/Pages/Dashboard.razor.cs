using ChargeLog.Models;

namespace ChargeLog.Pages
{
    public partial class Dashboard : IDisposable
    {
        private int _itemsDisplayedMax;
        private int _itemsDisplayed;
        private int _currentMonth;
        private List<DashboardMainTableRow> _tableRows = new List<DashboardMainTableRow>();
        
        protected override async Task OnInitializedAsync()
        {
            State = AppState.DashboardState;
            State.OnChange += ReloadDashboardAsync;
            Config = ConfigService.GetInterfaceConfig();
            var totalsRow = await ChargeLogService.GetTotalsAsync();            
            _itemsDisplayedMax = Config.MonthGroupSize;
            _tableRows.Add(totalsRow);
            await LoadMonthsAsync();

        }

        private async Task AddMonthGroupAsync()
        {
            _itemsDisplayedMax += Config.MonthGroupSize;
            await LoadMonthsAsync();
        }

        private async Task LoadMonthsAsync()
        {
            while (_itemsDisplayed < _itemsDisplayedMax)
            {
                var monthRow = await ChargeLogService.GetTotalsAsync(_currentMonth);
                _tableRows.Add(monthRow);
                _currentMonth--;
                _itemsDisplayed++;
            }
        }

        private async void ReloadDashboardAsync()
        {
            var totalsRow =  await ChargeLogService.GetTotalsAsync();
            _tableRows.Clear();
            _tableRows.Add(totalsRow);
            _currentMonth = 0;
            _itemsDisplayed = 0;
            await LoadMonthsAsync();
            if (State.OpenItems != null)
            {
                ReloadChildren(State.OpenItems);
            }
            StateHasChanged();
        }

        private void ReloadChildren(Dictionary<int, LevelState> childrenList)
        {
            foreach (var childEntry in childrenList)
            {
                var child = childEntry.Value;

                child.NotifyStateChanged();
                if (child.OpenItems != null)
                { 
                    ReloadChildren(child.OpenItems);
                }
            }
        }
        public void Dispose()
        {
            State.OnChange -= ReloadDashboardAsync;
        }
    }
}
