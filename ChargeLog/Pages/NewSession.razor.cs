using ChargeLog.DBModels;
using ChargeLog.ViewModels;
using Microsoft.AspNetCore.Components;

namespace ChargeLog.Pages
{
    public partial class NewSession
    {
        [Parameter]
        public int NetworkId { get; set; }

        [Parameter]
        public int LocationId { get; set; }

        private SessionView newSession = new SessionView();
        private Network? network;
        private List<Network> partnerNetworks = new List<Network>();
        private List<Car> cars = new List<Car>();
        private bool fullDiscount = false;

        protected override async Task OnInitializedAsync()
        {
            network = await ChargeLogService.GetNetworkAsync(NetworkId);
            cars = await ChargeLogService.GetCarsAsync();

            if (network != null && !network.HaveAccount)
            {
                partnerNetworks = await ChargeLogService.GetNetworksPartnerWithAccount();
            }
        }

        private void UpdatePrice()
        {
            if (network != null && network.Rate > 0)
            {
                double kWh;
                double.TryParse(newSession.KWh, out kWh);

                newSession.Price = (kWh * network.Rate).ToString();
            }
        }

        private void DiscountUpdate()
        {
            newSession.Discount = !fullDiscount ? newSession.Price : string.Empty;
        }

        private async Task AddSession()
        {
            double kWh = 0, price = 0, discount = 0;

            double.TryParse(newSession.KWh, out kWh);
            double.TryParse(newSession.Price, out price);
            double.TryParse(newSession.Discount, out discount);

            if (cars.Count == 1)
            {
                var car = cars.FirstOrDefault();
                if (car != null)
                {
                    newSession.CarId = car.Id;
                }
            }

            var session = new Session()
            {
                Date = DateTime.Parse(newSession.Date),
                Duration = TimeSpan.Parse(newSession.Duration ?? "00:00"),
                KWh = kWh,
                Price = price,
                Discount = discount,
                ChargeType = newSession.ChargeType,
                LocationId = LocationId,
                CarId = newSession.CarId
            };

            if (network != null && !network.HaveAccount)
            {
                session.ThroughNetworkId = newSession.ThroughNetworkId;
            }

            await ChargeLogService.AddSessioonAsync(session);
            AppState.DashboardState.NotifyStateChanged();

            newSession = new SessionView();
        }
    }
}
