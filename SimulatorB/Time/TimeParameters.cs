namespace SimulatorB
{
    internal static class TimeParameters
    {
        internal static readonly int HoursPerDay = 8;
        internal static readonly int DaysPerWeek = 5;
        internal static readonly int DaysPerMonth = 22;
        internal static readonly int WeeksPerYear = 44;

        internal static readonly int OPDurationInHours = 8;
        internal static readonly int OPMinimumMonthNotice = 4;
        
        internal static int HoursPerWeek { get => HoursPerDay * DaysPerWeek; }
        internal static int HoursPerMonth { get => HoursPerDay * DaysPerMonth; } 
        internal static int HoursPerYear { get => HoursPerDay * DaysPerWeek * WeeksPerYear; }
    }
}