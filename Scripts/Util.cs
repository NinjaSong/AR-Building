using System;

public static class Util
{
    private static readonly string SQL_FORMAT = "s";
    private static readonly string READABLE_FORMAT = "g";

    public static float Smootherstep(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    public static string CurrentSQLTimestamp()
    {
        return DateTime.Now.ToString(SQL_FORMAT);
    }

    public static string CurrentReadableTimestamp()
    {
        return DateTime.Now.ToString(READABLE_FORMAT);
    }

    public static string SQLTimestampToReadableTimestamp(string timestamp)
    {
        DateTime time;

        if (DateTime.TryParse(timestamp, out time))
            return time.ToString(READABLE_FORMAT);
        else
            return "";
    }
}