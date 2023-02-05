using ChargeLog.DBModels;
using ChargeLog.Models;
using Microsoft.AspNetCore.Components;

namespace ChargeLog.Pages
{
    public partial class GroupSessionList
    {
        [Parameter]
        public int GroupId { get; set; }

        private List<SessionGroupListItem> _sessionList = new List<SessionGroupListItem>();

        protected override async Task OnInitializedAsync()
        {
            _sessionList = await ChargeLogService.GetSessionsByGroupAsync(GroupId);
            Config = ChargeLogService.GetConfig();
        }
    }
}
