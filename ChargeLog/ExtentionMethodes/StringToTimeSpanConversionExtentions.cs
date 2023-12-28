namespace ChargeLog.ExtentionMethodes
{
    public static class GetHourExtention
    {
        public static string GetHour(this string timeStr)
        {
            return timeStr.IndexOf("h") > -1 ? timeStr.Substring(0, timeStr.IndexOf("h")) : "0";
        }
    }

    public static class GetMinutesExtention
    {
        public static string GetMinutes(this string timeStr)
        {
            if (timeStr.IndexOf("m") == -1)
            {
                return "0";
            }

            return timeStr.IndexOf("h") == -1 ? timeStr.Substring(0, timeStr.IndexOf("m")) :
                timeStr.Substring(timeStr.IndexOf("h") + 2, timeStr.IndexOf("m") - timeStr.IndexOf("h") - 2);
        }
    }
}
