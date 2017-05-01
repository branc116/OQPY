using System;
using System.Linq;

namespace OQPYModels.Helper
{
    public static class Helper
    {
        private static Random rand = new Random();
        public static int StringLikenes(this string _, string evalString)
        {
            int score = 0;
            string temp1 = evalString.Substring(0, 10<= evalString.Length ? 10 : evalString.Length).ToLower();
            string temp2 = _.ToLower();
            for ( int i = 1 ; i <= temp1.Length ; i++ )
            {
                for ( int j = 0 ; j <= (temp1.Length - i) ; j++ )
                {
                    try
                    {
                        score += (temp2.Contains(temp1.Substring(j, i))) ? i * (int)Math.Pow(2, 10 - temp1.Length + i) : 0;
                    }catch(Exception ex )
                    {
                        throw new Exception($"i = {i} j = {j} tempstr(j - 1, i - 1) = {temp1.Substring(j - 1, i - 1)}\n");
                    }
                }
            }
            return score / _.Length;
        }
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