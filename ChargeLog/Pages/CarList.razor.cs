using ChargeLog.DBModels;
using ChargeLog.ViewModels;

namespace ChargeLog.Pages
{
    public partial class CarList
    {
        private List<Car> cars = new List<Car>();
        private CarView newCar = new CarView();

        protected override async Task OnInitializedAsync()
        {
            cars = await ChargeLogService.GetCarsAsync();
        }

        private async Task AddCar()
        {
            var car = new Car()
            {
                Year = newCar.Year,
                Make = newCar.Make,
                Model = newCar.Model
            };

            await ChargeLogService.AddCarAsync(car);

            cars = await ChargeLogService.GetCarsAsync();

            newCar = new CarView();
        }
    }
}
