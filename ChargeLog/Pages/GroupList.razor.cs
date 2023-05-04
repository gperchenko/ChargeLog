using ChargeLog.Models;
using ChargeLog.DBModels;
using ChargeLog.ViewModels;

namespace ChargeLog.Pages
{
    public partial  class GroupList
    {
        private List<GroupListItem> groups = new List<GroupListItem>();
        private GroupView newGroup = new GroupView();

        protected override async Task OnInitializedAsync()
        {
            groups = await ChargeLogService.GetGroupsTotalsAsync();
            Config = ConfigService.GetInterfaceConfig();
        }

        private async Task AddGroup()
        {
            var group = new Group()
            {
                Name = newGroup.Name
            };

            await ChargeLogService.AddGroupAsync(group);

            groups = await ChargeLogService.GetGroupsTotalsAsync();

            newGroup = new GroupView();
        }
    }
}
