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
                network.Locations = null;
                networkList.Add(network);
            }

            return networkList;
        }
    }
}
