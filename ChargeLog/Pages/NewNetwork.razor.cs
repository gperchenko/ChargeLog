using ChargeLog.DBModels;
using ChargeLog.ViewModels;

namespace ChargeLog.Pages
{
    public partial class NewNetwork
    {
        private NetworkView newNetworkView = new NetworkView();

        protected async Task AddNetwork()
        {
            Double rate = 0;

            Double.TryParse(newNetworkView.Rate, out rate);

            var provider = new Network()
            {
                Name = newNetworkView.Name,
                Rate = rate,
                DefaultChargeType = newNetworkView.DefaultChargeType,
                HaveAccount = newNetworkView.HaveAccount,
                IsPartner = newNetworkView.IsPartner
            };

            await ChargeLogService.AddNetworkAsync(provider);
            AppState.DashboardState.NotifyStateChanged();

            newNetworkView = new NetworkView();
        }
    }
}
