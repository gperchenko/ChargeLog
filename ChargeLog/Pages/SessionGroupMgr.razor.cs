using ChargeLog.Models;

namespace ChargeLog.Pages
{
    public partial class SessionGroupMgr
    {
        protected string[] SelectedAvailableGroups { get; set; } = new string[0];
        protected string[] SelectedIncludedGroups { get; set; } = new string[0];
        protected List<KeyValue> AvailableGroups { get; set; } = new List<KeyValue>();
        protected List<KeyValue> IncludedGroups { get; set; } = new List<KeyValue>();

        protected override async Task OnInitializedAsync()
        {            
            IncludedGroups  = ChargeLogService.GetGroupsBySession();
            var groupList = ChargeLogService.GetAllGroups();
            AvailableGroups = groupList.Where(g => !IncludedGroups.Any(a => a.Key == g.Key)).ToList();
        }

        protected bool AddDisabled()
        {
            return SelectedAvailableGroups.Length == 0;
        }

        protected bool RemoveDisabled()
        {
            return SelectedIncludedGroups.Length == 0;
        }

        protected void AddNewGroups()
        {
            foreach (var group in SelectedAvailableGroups) 
            {
                ChargeLogService.AddGroupToSession();
            }
        }

        protected void RemoveGroups() 
        { 
            foreach(var group in SelectedIncludedGroups) 
            { 
                ChargeLogService.RemoveGroupFromSession();
            }
        }
    }
}
