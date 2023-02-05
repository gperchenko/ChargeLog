using ChargeLog.DBModels;

namespace ChargeLog.ExtentionMethodes
{
    public static class FilterSessionsExtension
    {
        public static IEnumerable<Session> FilterNetworks(this IEnumerable<Session> sessions, int monthOffset)
        {
            if (monthOffset > 0)
                return sessions;

            var sessionList = new List<Session>();

            foreach (var session in sessions)
            {
                sessionList.Add(session);
            }

            return sessionList;
        }
    }
}
