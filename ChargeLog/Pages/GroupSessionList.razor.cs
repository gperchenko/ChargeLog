using ChargeLog.DBModels;
using ChargeLog.Models;

namespace ChargeLog.Pages
{
    public partial class GroupSessionList
    {
        private List<SessionGroupListItem> _sessionList = new List<SessionGroupListItem>();

        protected override async Task OnInitializedAsync()
        {
            _sessionList = ChargeLogService.GetSessionGroupList();
            Config = ChargeLogService.GetConfig();
        }
    }
}
