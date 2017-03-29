using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OQPYModels.TestObjects
{
    public static class TestObjects
    {
        private static readonly List<string> _allImages = new List<string>() {
            "http://img.zanda.com/item/34070540000032/1024x768/Venue-Nightclub-Houston.jpg",
            "http://www.luxuryscottishwedding.co.uk/assets/venue/518bc00e9b9ed.jpg",
            "https://www.bristolmuseums.org.uk/wp-content/uploads/2015/03/BMAG_interior_venue-677x380.jpg",
            "http://www.theplaceaparthotel.com/uploads/PageImage/328.jpg"
        };


        public static List<string> VenueImagesTest
        {
            get
            {
                return _allImages;
            }
        }
        public static List<BaseTag> TagsTest
        {
            get
            {
                var rand = new Random(DateTime.Now.Millisecond);
                return (from _ in new string(' ', 30)
                        let name = new string((from __ in new string('a', 5)
                                               select (char)(__ + rand.Next(0, 24))).ToArray())
                        let id = Guid.NewGuid().ToString()
                        select new BaseTag() { Id = id, TagName = name }).ToList();
            }
        }
        public static List<string> NamesTest
        {
            get
            {
                var rand = new Random(DateTime.Now.Millisecond);
                return (from _ in new string(' ', 5)
                        let name = new string((from __ in new string('a', rand.Next(4, 10))
                                               select (char)(__ + rand.Next(0, 24))).ToArray())
                        select name).ToList();
            }
        }
        public static List<BaseVenue> VenuesTest
        {
            get
            {
                var rand = new Random(DateTime.Now.Millisecond);
                var names = NamesTest;
                var tags = TagsTest;
                var images = VenueImagesTest;
                var venus = from _ in NamesTest
                            let venuetags = from __ in new string(' ', rand.Next(4, 7))
                                            select TagsTest[rand.Next(0, TagsTest.Count)]
                            let image = images[rand.Next(0, images.Count)]
                            select new BaseVenue()
                            {
                                Tags = venuetags.ToList(),
                                Id = new Guid().ToString(),
                                ImageUrl = image,
                                Name = _
                            };
                return venus.ToList();
            }
        }
    }
}
