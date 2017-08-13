namespace Simulator
{
    internal static class TimeParameters
    {
        internal static int HoursPerDay = 8;
        internal static int DaysPerWeek = 5;
        internal static int DaysPerMonth = 22;
        internal static int WeeksPerYear = 44;

        internal static int OPDurationInHours = 8;
        internal static int OPMinimumMonthNotice = 4;
        

        internal static int HoursPerMonth { get { return HoursPerDay * DaysPerMonth; } }
        internal static int HoursPerYear { get { return HoursPerDay * DaysPerWeek * WeeksPerYear; } }
    }
}