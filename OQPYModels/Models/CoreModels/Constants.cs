namespace OQPYModels.Models.CoreModels
{
    public static class Constants
    {
        public static int MinimumRating = 0;
        public static int MaximumRating = 10;
        public static double EarthRadius = 6378.137;
    }

    public static class ErrorMessages
    {
        public static string Reservation2Long = "One can only reservate ones resource during one day";
        public static string ReservatioAlredyTaken = "alredy taken in this time";
        public static string ClosedInThisTime = "Not working in this time";
    }
}