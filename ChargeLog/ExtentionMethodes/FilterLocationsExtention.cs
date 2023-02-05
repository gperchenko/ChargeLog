using ChargeLog.DBModels;

namespace ChargeLog.ExtentionMethodes
{
    public static class FilterLocationsExtension
    {
        public static IEnumerable<Location> FilterNetworks(this IEnumerable<Location> locations, int monthOffset)
        {
            if (monthOffset > 0)
                return locations;

            var locationList = new List<Location>();

            foreach (var location in locations)
            {
                location.Sessions = null;
                locationList.Add(location);
            }

            return locationList;
        }
    }
}
