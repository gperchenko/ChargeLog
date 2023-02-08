using ChargeLog.DBModels;

namespace ChargeLog.ExtentionMethodes
{
    public static class FilterLocationsExtension
    {
        public static IEnumerable<Location> FilterLocations(this IEnumerable<Location> locations, int monthOffset)
        {
            if (monthOffset > 0)
                return locations;

            var locationList = new List<Location>();

            foreach (var location in locations)
            {
                if (location.Sessions == null) continue;
                var sessions = location.Sessions.FilterSessions(monthOffset);
                if (sessions.Count() > 0)
                {
                    location.Sessions = sessions.ToList();
                    locationList.Add(location);
                }               
            }

            return locationList;
        }
    }
}
