using ChargeLog.Models;
using ChargeLog.Services;
using Microsoft.AspNetCore.Components;

namespace ChargeLog.Pages
{
    public class PageBase : ComponentBase
    {
        [Inject]
        required public IChargeLogService ChargeLogService { get; init; }

        [Inject]
        required public IImportService ImportService { get; init; }

        [Inject]
        required public IConfigService ConfigService { get; init; }

        [Inject]
        required public AppState AppState { get; init; }

        protected InterfaceConfig Config = new InterfaceConfig();
        protected LevelState State = new();

        protected void TuggleShowDetails(LevelState state, int idx)
        {
            if (state.OpenItems == null)
            {
                state.OpenItems = new();
            }

            if (state.OpenItems.ContainsKey(idx))
            {
                state.OpenItems.Remove(idx);
            }
            else
            {
                state.OpenItems.Add(idx, new LevelState());
            }
        }

        protected bool IsShowDetails(LevelState state, int idx)
        {
            if (state.OpenItems == null) return false;

            return state.OpenItems.ContainsKey(idx);
        }

        protected void TuggleVisibleForm(LevelState state)
        {
            state.ShowForm = !state.ShowForm;
        }
    }
}
