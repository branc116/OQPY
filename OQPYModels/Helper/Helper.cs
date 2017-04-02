using System;
using System.Linq;

namespace OQPYModels.Helper
{
    public static class Helper
    {
        private static Random rand = new Random();

        public static string RandomName()
        {
            return RandomText(5, 9);
        }

        public static string RandomEmail()
        {
            return $"{RandomName()}@gmail.com";
        }

        public static string RandomUriOfVenue()
        {
            return TestObjects.TestObjects.VenueImagesTest[rand.Next(0, TestObjects.TestObjects.VenueImagesTest.Count)];
        }

        public static string RandomText(int minWords, int maxWords)
        {
            return new string((from _ in new string('a', rand.Next(minWords, maxWords))
                               select ((char)(_ + rand.Next(0, 25)))).ToArray());
        }

        public static decimal RandomDecimal(decimal min, decimal max)
        {
            return min + (max - min) * (decimal)rand.NextDouble();
        }

        public static double RandomDouble(double min, double max)
        {
            return min + (max - min) * rand.NextDouble();
        }

        public static int RandomInt(int min, int max)
        {
            return rand.Next(min, max);
        }

        /// <summary>
        /// int days
        /// </summary>
        /// <returns></returns>
        public static TimeSpan RandomDays(double minDays, double maxDays)
        {
            return TimeSpan.FromDays(RandomDouble(minDays, maxDays));
        }

        public static TimeSpan RandomHours(double minHours, double maxHours)
        {
            return TimeSpan.FromHours(RandomDouble(minHours, maxHours));
        }

        public static bool RandomBool() => rand.Next(0, 10) < 5;
    }
}