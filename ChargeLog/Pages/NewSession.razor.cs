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

        [Parameter]
        public int MonthOffset { get; set; }

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

            // fix date if tthis is for dirrent month 

            if (MonthOffset < 0)
            {
                var currentDate = DateTime.Now;
                var newDate = currentDate.AddMonths(MonthOffset);
                newSession.Date = new DateTime(newDate.Year, newDate.Month, 1).ToShortDateString();
            }
        }

        private void UpdatePrice()
        {
            if (network != null && network.Rate > 0)
            {
                decimal kWh;
                decimal.TryParse(newSession.KWh, out kWh);

                newSession.Price = (kWh * network.Rate).ToString();
            }
        }

        private void DiscountUpdate()
        {
            newSession.Discount = !fullDiscount ? newSession.Price : string.Empty;
        }

        private async Task AddSession()
        {
            decimal kWh = 0, price = 0, discount = 0;

            decimal.TryParse(newSession.KWh, out kWh);
            decimal.TryParse(newSession.Price, out price);
            decimal.TryParse(newSession.Discount, out discount);

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
