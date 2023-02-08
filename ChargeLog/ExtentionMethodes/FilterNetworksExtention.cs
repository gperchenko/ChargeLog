using ChargeLog.DBModels;

namespace ChargeLog.ExtentionMethodes
{
    public static class FilterNetworksExtention
    {
        public static IEnumerable<Network> FilterNetworks(this IEnumerable<Network> networks, int monthOffset)
        {
            if (monthOffset > 0)
                return networks;

            var networkList = new List<Network>();

            foreach (var network in networks)
            {
                if (network.Locations == null) continue;
                var locations = network.Locations.FilterLocations(monthOffset);
                if (locations.Count() > 0)
                {
                    network.Locations = locations.ToList();
                    networkList.Add(network);
                }              
            }

            return networkList;
        }
    }
}
