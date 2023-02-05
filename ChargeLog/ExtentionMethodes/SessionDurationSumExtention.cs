using ChargeLog.DBModels;

namespace ChargeLog.ExtentionMethodes
{
    public static class SessionDurationSumExtention
    {
        public static TimeSpan DurationSum(this IEnumerable<Session> sessions)
        {
            TimeSpan result = TimeSpan.Zero;

            foreach (var session in sessions)
            {
                result += session.Duration;
            }

            return result;
        }
    }
}
