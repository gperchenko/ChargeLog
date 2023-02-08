using ChargeLog.DBModels;

namespace ChargeLog.ExtentionMethodes
{
    public static class FilterSessionsExtension
    {
        public static IEnumerable<Session> FilterSessions(this IEnumerable<Session> sessions, int monthOffset)
        {
            if (monthOffset > 0)
                return sessions;

            var nowDate = DateTime.Now;
            var targetDate = nowDate.AddMonths(monthOffset);

            return sessions.Where(s => s.Date.Month == targetDate.Month && s.Date.Year == targetDate.Year);
        }
    }
}
