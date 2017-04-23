using GoogleApi;
using GoogleApi.Entities.Places.Search.NearBy.Request;
using OQPYClient.APIv03;
using OQPYModels.Models.CoreModels;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace OQPYCralwer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var cralw = new Cralw();
            Task.WaitAll(cralw.CralwBars());
        }
    }
    public class Cralw
    {
        private static string _apiKey = "AIzaSyDILEdR5gAKYwZKyocx1nsKOhQev5QQ68Q";

        public Cralw()
        {
        }


        public async Task CralwBars()
        {
            var req = new PlacesNearBySearchRequest()
            {
                Key = "AIzaSyDILEdR5gAKYwZKyocx1nsKOhQev5QQ68Q",
                Location = new GoogleApi.Entities.Common.Location(45.7857986, 15.948076099999998),
                Type = GoogleApi.Entities.Places.Search.Common.Enums.SearchPlaceType.Bar,
                Radius = 10000,
                Sensor = true
            };
            var res = await GooglePlaces.NearBySearch.QueryAsync(req);
            var resTask = from _ in res.Results
                          let v = new GoogleApi.Entities.Places.Details.Request.PlacesDetailsRequest()
                          {
                              PlaceId = _.PlaceId,
                              Key = _apiKey
                          }
                          select GooglePlaces.Details.QueryAsync(v);
            var res2 = await Task.WhenAll(resTask);
            var api = new MyAPI(new Uri("http://localhost:5000/"));
            var containd = await api.ApiVenuesMultiGetAsync((from _ in res2
                                                             select _.Result.PlaceId).ToList());
            var exes = res2.Select(i =>
            {
                if ( containd.All(ii => ii.Id != i.Result.PlaceId) )
                    return i.Result;
                return null;
            });
            var newVenues = from _ in exes
                            let venue = new Venue(_.Name, null, _.Icon, _.FormattedAddress, _.PlaceId)
                            {
                                Description = _.Website,
                                VenueCreationDate = DateTime.Now,
                            }
                            let reviews = venue.Reviews = _.Review != null ? (from __ in _.Review
                                                                              select new Review((int)(__.Rating * 2), __.Text, venue)).ToList() : null
                            let tags = venue.Tags = _.Types != null ? (from __ in _.Types
                                                                       select new Tag(__.ToString(), venue)).ToList() : null
                            let workh = venue.WorkHours = _.OpeningHours.ToNormalWorkHovers(venue)
                            select venue.UnFixLoops();

            await api.ApiVenuesMultiFullPostAsync(newVenues.ToList());
        }
    }

    public static class Ext
    {
        public static WorkHours ToNormalWorkHovers(this GoogleApi.Entities.Places.Details.Response.OpeningHours p, Venue venue)
        {
            if ( p == null )
                return null;
            var vh = new WorkHours(null, p.OpenNow, venue);
            vh.WorkTimes = (from _ in p.Periods
                            let start = DateTime.MinValue.AddHours((double)(Convert.ToInt32(_.Open?.Time ?? "0") / 100)).AddMinutes((double)(Convert.ToInt32(_.Open?.Time ?? "0") % 100))
                            let end = DateTime.MinValue.AddHours((double)(Convert.ToInt32(_.Close?.Time ?? "0") / 100)).AddMinutes((double)(Convert.ToInt32(_.Close?.Time ?? "0") % 100))
                            select new WorkTime(start, end, vh)).ToList();
            return vh;
        }
    }
}