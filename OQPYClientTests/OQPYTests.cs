//#define prod
#define console

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Rest.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OQPYClient.APIv03;
using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static OQPYModels.Helper.Helper;

namespace OQPYClientTests
{
    [TestClass]
    public class OQPYTests
    {
        private const string filename = @"C:\Users\Branimir\Logs\debug.log";

        private MyAPI api = new MyAPI(
#if prod
                new Uri("https://oqpymanager.azurewebsites.net/")
#elif console
                new Uri("http://localhost:5000")
#else
                new Uri("http://localhost:52305/")
#endif
                );

        [TestMethod]
        public async Task CrawlByText()
        {
            DropLog();
            var cralwer = new OQPYCralwer.Cralw();
            var venues = await cralwer.CrawlByText("Havana");
            AssertVenues(venues);
        }

        [TestMethod]
        public async Task LocationNameManager()
        {
            IList<Venue> venues;

            venues = await api.ApiVenuesFilterPostAsync((new Venue()
            {
                Location = new Location(45.7849274, 15.9468622),
                Name = "Studentski dom"
            }));
            Log(venues.Count.ToString());
            Log(venues.First().ToString());
            AsserVenues(venues);
        }

        [TestMethod]
        public async Task HelloTest()
        {
            IList<Venue> venues;

            venues = await api.ApiVenuesFilterPostAsync(new OQPYModels.Models.CoreModels.Venue() { Location = new OQPYModels.Models.CoreModels.Location() { Longditude = -74.0059731, Latitude = 40.7143528 } });
            AsserVenues(venues);
        }

        [TestMethod]
        public async Task TestingCrawlingByLocation()
        {
            var cralwer = new OQPYCralwer.Cralw();
            var venues = await cralwer.CrawlByLocation(new GoogleApi.Entities.Common.Location(40.7143528, -74.0059731));
            AssertVenues(venues);
        }

        [TestMethod]
        public async Task TestingCrawlingByLocationAndName()
        {
            var cralwer = new OQPYCralwer.Cralw();
            var venues = await cralwer.CrawlSimlar(new Venue()
            {
                Location = new Location(15.9474278, 45.7851303),
                Name = "Retro"
            });
            AssertVenues(venues);
        }

        [TestMethod]
        public async Task CrowlByText()
        {
            var cralwer = new OQPYCralwer.Cralw();
            var venues = await cralwer.CrawlByText("bars");
            AssertVenues(venues);
        }

        [TestMethod]
        public async Task MenagerFilterTestNameOnly()
        {
            IList<Venue> venues;
            venues = await api.ApiVenuesFilterPostAsync(new OQPYModels.Models.CoreModels.Venue() { Name = "Bars" });
            AsserVenues(venues);
        }

        [TestMethod]
        public async Task AddressToLocation()
        {
            var placesToVisit = new List<string>() { "zagreb", "istandbul", "knežija", "fer", "transy", "budapeste", "Konjščina" };
            var str = string.Empty;
            foreach ( var place in placesToVisit )
            {
                var crawler = new OQPYCralwer.Cralw();
                var locs = await crawler.AddressToLocation(place);
                str += place + "\n";
                foreach ( var loc in locs )
                {
                    str += loc.ToString() + "\n";
                    Assert.AreNotEqual(loc.Latitude, 0);
                    Assert.AreNotEqual(loc.Longitude, 0);
                }
            }
            //Log(str);
        }

        [TestMethod]
        public void TestingStringLiknes()
        {
            var temp1 = "stringyString";
            var temp2 = "string";
            temp1.StringLikenes(temp2);
            temp2.StringLikenes(temp1);
            Random rand = new Random();

            for ( int i = 0 ; i < 10 ; i++ )
            {
                temp1 = new string((from _ in new string('a', rand.Next(10, 100))
                                    select (char)(_ + rand.Next(0, 25))).ToArray());
                temp2 = new string((from _ in new string('a', rand.Next(10, 100))
                                    select (char)(_ + rand.Next(0, 25))).ToArray());
                temp1.StringLikenes(temp2);
                temp2.StringLikenes(temp1);
            }
        }

        [TestMethod]
        public async Task TestChangeState()
        {
            await api.ApiResourcesIOTPostWithHttpMessagesAsync("Zagreb_stijepanradic_8_30_1", "1", null);
        }

        public static void AssertVenues(IEnumerable<Venue> venues)
        {
            Assert.IsNotNull(venues);
            foreach ( var venue in venues )
            {
                Assert.IsNotNull(venue.ImageUrl);
                Assert.IsNotNull(venue.Location);
                Assert.IsNotNull(venue.Name);
                Assert.AreNotEqual(venue.Location.Latitude, 0);
                Assert.AreNotEqual(venue.Location.Longditude, 0);
            }
            AssertBotHelpers(venues);
        }

        public static void AsserVenues(IList<Venue> venues)
        {
            Assert.IsNotNull(venues);
            Assert.AreNotEqual(venues.Any(), false);
            //Logger.LogMessage($"{venues.Count}");
            var str = string.Empty;
            for ( int i = 0 ; i < (venues.Count - 1) ; i++ )
            {
                str += $"{i}: {venues[i].ToString()}Distance to original: \n\n";
                Assert.AreNotEqual(venues[i].Id, venues[i + 1].Id);
            }
            AssertBotHelpers(venues);
            //Log(str);
            Assert.AreEqual(1, 1);
        }

        private IEnumerable<Activity> GetActivitiesFromFile(string path)
        {
            var delimiter = "^^ˇˇ\n";
            var jsons = File.ReadAllText(path).Split(new string[1] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
            return from _ in jsons
                   select SafeJsonConvert.DeserializeObject<Activity>(_, OQPYBot.Controllers.Helper.Constants._safeDeserializationSettings);
        }

        public static Queue<string> DebugOut = new Queue<string>();

        public static void BumpLog()
        {
            lock ( DebugOut )
            {
                if ( !File.Exists(filename) )
                    File.Create(filename);
                File.AppendAllText(filename, DebugOut.Dequeue() + "\n");
            }
        }

        public static void Log(string content)
        {
            DebugOut.Enqueue($"{content}\n\n");
            BumpLog();
        }

        public static void DropLog()
        {
            lock ( DebugOut )
            {
                File.Move(filename, filename.Split(new string[1] { ".log" }, StringSplitOptions.RemoveEmptyEntries)[0] + DateTime.Now.ToString("ssmmhhddMMyyyy") + ".log");
                File.Create(filename);
            }
        }

        public static void AssertBotHelpers(IEnumerable<Venue> venues)
        {
            var cards = OQPYBot.Controllers.Helper.Helper.MakeACard(venues);
            Assert.IsNotNull(cards);
            Assert.AreNotEqual(cards.Count(), 0);
            foreach ( var card in cards )
            {
                Log(card.Content + " " + card.ContentType + " " + card.Name + " ");
                Assert.IsNotNull(card.Content);
                Assert.IsNotNull(card.ContentType);
                //Assert.IsNotNull(card.Name);
            }
        }
    }

    [TestClass]
    public class TestringModels
    {
        [TestMethod]
        public void TestringLocationFilter()
        {
            var locationOne = new Location(0, 0);
            var locationTwo = new Location(100, 100);
            Assert.AreEqual(locationOne.Filter(locationOne, locationTwo), false);
            locationOne = null;
            Assert.AreEqual(locationTwo.Filter(locationOne, locationTwo), false);
            locationTwo = new Location(0.01, 0.01);
            Assert.AreEqual(locationTwo.Filter(locationOne, locationTwo), false);
        }

        [TestMethod]
        public void TestingTokilometers()
        {
            var locationOne = new Location(0, 0);
            var locationTwo = new Location(0, 0);
            var distance = locationOne.ToKilometers(locationTwo);
            Assert.AreEqual(distance, 0);
        }
    }
}