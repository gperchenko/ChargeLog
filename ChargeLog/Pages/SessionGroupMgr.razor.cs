using ChargeLog.Models;
using Microsoft.AspNetCore.Components;

namespace ChargeLog.Pages
{
    public partial class SessionGroupMgr
    {
        [Parameter]
        public int SessionId { get; set; }

        protected string[] SelectedAvailableGroups { get; set; } = new string[0];
        protected string[] SelectedIncludedGroups { get; set; } = new string[0];
        protected List<KeyValue> AvailableGroups { get; set; } = new List<KeyValue>();
        protected List<KeyValue> IncludedGroups { get; set; } = new List<KeyValue>();
        protected List<KeyValue> IncludedGroupsNotFiltered { get; set; } = new List<KeyValue>();


        protected override async Task OnInitializedAsync()
        {            
            IncludedGroups  = await ChargeLogService.GetGroupsBySessionAsync(SessionId);
            IncludedGroupsNotFiltered = await ChargeLogService.GetAllGroupsAsync();
            AvailableGroups = IncludedGroupsNotFiltered.Where(g => !IncludedGroups.Any(a => a.Key == g.Key)).ToList();
        }

        protected bool AddDisabled()
        {
            return SelectedAvailableGroups.Length == 0;
        }

        protected bool RemoveDisabled()
        {
            return SelectedIncludedGroups.Length == 0;
        }

        protected async Task AddNewGroups()
        {
            var SelectedAvailableGroupsInt = Array.ConvertAll(SelectedAvailableGroups, int.Parse);

            foreach (var groupId in SelectedAvailableGroupsInt) 
            {
                await ChargeLogService.AddGroupToSessionAsync(SessionId, groupId);
            }

            IncludedGroups = await ChargeLogService.GetGroupsBySessionAsync(SessionId);
            AvailableGroups = IncludedGroupsNotFiltered.Where(g => !IncludedGroups.Any(a => a.Key == g.Key)).ToList();
        }

        protected async Task RemoveGroups() 
        {
            var SelectedIncludedGroupsInt = Array.ConvertAll(SelectedIncludedGroups, int.Parse);

            foreach (var groupId in SelectedIncludedGroupsInt)
            {
                await ChargeLogService.RemoveGroupFromSessionAsync(SessionId, groupId);
            }

            IncludedGroups = await ChargeLogService.GetGroupsBySessionAsync(SessionId);
            AvailableGroups = IncludedGroupsNotFiltered.Where(g => !IncludedGroups.Any(a => a.Key == g.Key)).ToList();
        }
    }
}
