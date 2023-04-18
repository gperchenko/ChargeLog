using ChargeLog.DBModels;
using ChargeLog.Models;
using Microsoft.AspNetCore.Components;

namespace ChargeLog.Pages
{
    public partial class GroupSessionList
    {
        [Parameter]
        public int GroupId { get; set; }

        private List<SessionGroupListItem> _sessions = new List<SessionGroupListItem>();

        protected override async Task OnInitializedAsync()
        {
            _sessions = await ChargeLogService.GetSessionsByGroupAsync(GroupId);
            Config = ConfigService.GetConfig();
        }
    }
}
