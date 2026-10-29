namespace WebApp.Helpers;

public static class TimeFormatter
{
    public static string TimeAgo(DateTimeOffset date)
    {
        if (date == default)
            return "Unknown date";

        var elapsed = DateTimeOffset.UtcNow - date;

        if (elapsed.TotalSeconds < 60)
            return "just now";

        if (elapsed.TotalMinutes < 60)
            return $"{(int)elapsed.TotalMinutes} min ago";

        if (elapsed.TotalHours < 24)
            return $"{(int)elapsed.TotalHours} hours ago";

        if (elapsed.TotalDays < 7)
            return $"{(int)elapsed.TotalDays} days ago";

        if (elapsed.TotalDays < 30)
            return $"{(int)(elapsed.TotalDays / 7)} weeks ago";

        return date.ToString("dd MMM yyyy");
    }
}