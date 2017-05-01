using GoogleApi;
using GoogleApi.Entities.Places.Details.Response;
using GoogleApi.Entities.Places.Search.NearBy.Request;
using GoogleApi.Entities.Places.Search.NearBy.Response;
using GoogleApi.Entities.Places.Search.Text.Response;
using OQPYClient.APIv03;
using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Venue>> CrawlSimlar(Venue like)
        {
            var req = new PlacesNearBySearchRequest()
            {
                Key = _apiKey,
                Keyword = like.Name ?? like.Description ?? like.Tags.Select(i => i.TagName).Aggregate((i, j) => $"{i} {j}") ?? string.Empty,
                Location = like.Location != null ?
                           like.Location.Latitude != 0 && like.Location.Longditude != 0 ?
                                new GoogleApi.Entities.Common.Location(like.Location.Latitude, like.Location.Longditude) :
                                !string.IsNullOrWhiteSpace(like.Location.Adress) ? (await AddressToLocation(like.Location.Adress)).FirstOrDefault() :
                            null :
                            null,
                Radius = 10000
            };
            var res = await GooglePlaces.NearBySearch.QueryAsync(req);
            return await GetDetails(res.Results);
        }

        public async Task<IEnumerable<Venue>> CrawlByText(string text)
        {
            var req = new GoogleApi.Entities.Places.Search.Text.Request.PlacesTextSearchRequest()
            {
                Key = _apiKey,
                Query = text,
            };
            var res = await GooglePlaces.TextSearch.QueryAsync(req);
            return await GetDetails(res.Results);
        }

        public async Task<IEnumerable<Venue>> CrawlByLocation(string address)
        {
            var locations = await AddressToLocation(address);
            return await CrawlByLocation(locations.ToArray());
        }

        public async Task<IEnumerable<Venue>> CrawlByLocation(params GoogleApi.Entities.Common.Location[] locations)
        {
            var reqs = from _ in locations
                       let location = _
                       let req = new PlacesNearBySearchRequest()
                       {
                           Key = "AIzaSyDILEdR5gAKYwZKyocx1nsKOhQev5QQ68Q",
                           Location = location,
                           Radius = 1000,
                           Sensor = false
                       }
                       select GooglePlaces.NearBySearch.QueryAsync(req);
            var resoults = await Task.WhenAll(reqs);
            var details = await Task.WhenAll(from _ in resoults
                                             select GetDetails(_.Results));
            List<Venue> venues = new List<Venue>(details.Length * 10);
            foreach ( var _ in details )
                venues.AddRange(_);
            return venues;
        }

        public async Task<IEnumerable<Venue>> GetDetails(IEnumerable<NearByResult> response)
        {
            var resTask = from _ in response
                          let v = new GoogleApi.Entities.Places.Details.Request.PlacesDetailsRequest()
                          {
                              PlaceId = _.PlaceId,
                              Key = _apiKey
                          }
                          select GooglePlaces.Details.QueryAsync(v);
            var res2 = await Task.WhenAll(resTask);
            return CreateVenues(res2);
        }

        public async Task<IEnumerable<Venue>> GetDetails(IEnumerable<TextResult> response)
        {
            var resTask = from _ in response
                          let v = new GoogleApi.Entities.Places.Details.Request.PlacesDetailsRequest()
                          {
                              PlaceId = _.PlaceId,
                              Key = _apiKey
                          }
                          select GooglePlaces.Details.QueryAsync(v);
            var res2 = await Task.WhenAll(resTask);
            return CreateVenues(res2);
        }

        public IEnumerable<Venue> CreateVenues(params PlacesDetailsResponse[] places)
        {
            var newVenues = from res in places
                            where res.Status == GoogleApi.Entities.Common.Status.Ok
                            where res.Result != null
                            let _ = res.Result
                            let venue = new Venue(_.Name, null, _.Icon, _.FormattedAddress, _.PlaceId)
                            {
                                Description = _.Website,
                                VenueCreationDate = DateTime.Now,
                                Location = new Location() {
                                    Latitude = _.Geometry.Location.Latitude,
                                    Longditude = _.Geometry.Location.Longitude,
                                    Adress = _.FormattedAddress
                                }
                            }
                            let reviews = venue.Reviews = _.Review != null ? (from __ in _.Review
                                                                              select new OQPYModels.Models.CoreModels.Review((int)(__.Rating * 2), __.Text, venue)).ToList() : null
                            let tags = venue.Tags = _.Types != null ? (from __ in _.Types
                                                                       select new Tag(__.ToString(), venue)).ToList() : null
                            let workh = venue.WorkHours = _.OpeningHours.ToNormalWorkHovers(venue)
                           
                            select venue.UnFixLoops();
            return newVenues;
        }

        public async Task<IEnumerable<GoogleApi.Entities.Common.Location>> AddressToLocation(string address)
        {
            var req = new GoogleApi.Entities.Maps.Geocode.Request.GeocodingRequest()
            {
                Address = address,
                Key = _apiKey,
            };
            var res = await GoogleApi.GoogleMaps.Geocode.QueryAsync(req);
            return from _ in res.Results
                   where _.Geometry != null
                   where _.Geometry.Location != null
                   select _.Geometry.Location;
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
                                                                              select new OQPYModels.Models.CoreModels.Review((int)(__.Rating * 2), __.Text, venue)).ToList() : null
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