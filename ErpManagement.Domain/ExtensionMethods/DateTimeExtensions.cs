namespace ErpManagement.Domain.Extensions;

public static class DateTimeExtensions
{
    public static DateTime NowEg(this DateTime dateToCheck) => 
        DateTime.UtcNow.AddHours(2);

    public static IEnumerable<DateTime> GetDatesBetween(DateTime startDate, DateTime endDate)
    {
        var allDates = new List<DateTime>();
        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            if (date.DayOfWeek != DayOfWeek.Friday && date.DayOfWeek != DayOfWeek.Saturday)
                allDates.Add(date);
        return allDates;
    }

    public static int GetDaysOffCountForTheCurrentMonth(DateTime da)
    {
        //DateTime dateNow = DateTime.Today;
        var startDate = new DateTime(da.Year, da.Month, 1);
        var endDate = new DateTime(da.Year, da.Month, 1).AddMonths(1).AddDays(-1);
        int daysOffCount = 0;
        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            if (date.DayOfWeek == DayOfWeek.Friday || date.DayOfWeek == DayOfWeek.Saturday)
                daysOffCount++;
        return daysOffCount;
    }

    public static DateTime GetLastCurrentMonthDate(string? date)
    {
        var theDate = date == null ? DateTime.Today : Convert.ToDateTime(date);
        return new DateTime(theDate.Year, theDate.Month, 1).AddMonths(1).AddDays(-1);
    }

    public static string ToGetFullyDate(this DateTime date) =>
        date.ToString("yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture);
    public static string ToGetFullyDate(this DateTime? date) =>
        Convert.ToDateTime(date).ToString("yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture);

    public static string ToGetOnlyDate(this DateTime date) =>
        date.ToString("yyyy-MM-dd");
    public static string ToGetOnlyDate(this DateTime? date) =>
        Convert.ToDateTime(date).ToString("yyyy-MM-dd");

    public static string ToGetOnlyTime(this DateTime date) =>
        date.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);

    public static string ToGetOnlyDateInAlphabetic(this DateTime date) =>
        date.ToString("yyyy-MMMM-dd");

    public static string ToGetDayInArabic(this DateTime date) =>
        date.ToString("dddd", new CultureInfo("ar-AE"));

    public static string ToGetDayInArabic(this DateTime? date) =>
        Convert.ToDateTime(date).ToString("dddd", new CultureInfo("ar-AE"));

    public static string ToGetDayInEnglish(this DateTime date) =>
        date.ToString("dddd");

    public static string ToGetDayInEnglish(this DateTime? date) =>
        Convert.ToDateTime(date).ToString("dddd");

    public static DateTime? ToIncreaseOneHour(this DateTime? date) =>
        date == null ? date : date.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
}
