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
            State.OnChange += ReloadDashboard;
            Config = ChargeLogService.GetConfig();
            var totalsRow = await ChargeLogService.GetTotalsAsync();            
            _itemsDisplayedMax = Config.MonthGroupSize;
            _tableRows.Add(totalsRow);
            LoadMonths();

        }

        private void AddMonthGroup()
        {
            _itemsDisplayedMax += Config.MonthGroupSize;
            LoadMonths();
        }

        private void LoadMonths()
        {
            while (_itemsDisplayed < _itemsDisplayedMax)
            {
                var monthRow = ChargeLogService.GetMonth(_currentMonth);
                _tableRows.Add(monthRow);
                _currentMonth--;
                _itemsDisplayed++;
            }
        }

        private async void ReloadDashboard()
        {
            var totalsRow =  await ChargeLogService.GetTotalsAsync();
            _tableRows.Clear();
            _tableRows.Add(totalsRow);
            _currentMonth = 0;
            _itemsDisplayed = 0;
            LoadMonths();
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
            State.OnChange -= ReloadDashboard;
        }
    }
}
