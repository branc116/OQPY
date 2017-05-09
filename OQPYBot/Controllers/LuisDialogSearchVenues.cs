using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using OQPYClient.APIv03;
using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static OQPYBot.Controllers.Helper.Constants;

namespace OQPYBot.Controllers
{
    [LuisModel("2f4d5a10-e2cf-4238-ab65-51ab4b4dd0ea", "b36329fcaa154546ba25f10bc5740770")]
    [Serializable]
    public class LuisDialogSearchVenues
    {
        public static IDialog<SearchVenues> Create(IDialogContext context)
        {
            var form = Chain.From(() =>
            {
                return FormDialog.FromForm(SearchVenues.Builder);
            });
            var message = context.MakeMessage();
            return form;
        }
    }

    [Serializable]
    public class SearchVenues
    {
        [Prompt("What is the name of the venue? (n - nothing)")]
        public string Name { get; set; }

        //[Optional]
        //[Prompt("Were is this venue?")]
        //public string Address { get; set; }

        //[Optional]
        //public Venue Like { get => like; set => like = value; }
        public Venue GetVenue() => new Venue() { Name = Name };

        public async Task<IEnumerable<Venue>> QAsync(Geo location)
        {
            var api = new MyAPI(new Uri(_managerUri));
            var venue = GetVenue();
            if ( location != null )
            {
                venue.Location = new Location() { Latitude = location.latitude, Longditude = location.longitude };
                venue.Name = venue.Name.ToLower() == "n" ? null : venue.Name;
            }
            return await api.ApiVenuesFilterPostAsync(venue);
        }

        public async Task<IEnumerable<Venue>> QAsync(Venue like)
        {
            return await _api.ApiVenuesFilterPostAsync(like);
        }

        public static IForm<SearchVenues> Builder()
        {
            var colture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            var build = new FormBuilder<SearchVenues>()
                    .Message("Give me some information on what do you want to search")
                    .Field(nameof(Name));
            var a = build.Build();
            return a;
        }
    }
}