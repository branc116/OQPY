using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using OQPYClient.APIv03;
using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OQPYBot.Controllers
{
    [LuisModel("2f4d5a10-e2cf-4238-ab65-51ab4b4dd0ea", "b36329fcaa154546ba25f10bc5740770")]
    [Serializable]
    public class LuisDialogSearchVenues
    {
        public static IDialog<SearchVenues> Create(IDialogContext context)
        {
            return Chain.From(() =>
            {
                try
                {
                    return FormDialog.FromForm(SearchVenues.Builder);
                }
                catch ( Exception ex )
                {
                    throw;
                }
            });
        }
    }

    [Serializable]
    public class SearchVenues
    {
        [Optional]
        //[Prompt("What is the name of the venue?")]
        public string Name { get; set; }

        //[Optional]
        //[Prompt("Were is this venue?")]
        //public string Address { get; set; }

        //[Optional]
        //public Venue Like { get => like; set => like = value; }
        public Venue GetVenue() => new Venue() { Name = Name };

        public async Task<IEnumerable<Venue>> QAsync()
        {
            var api = new MyAPI(
#if DEBUG
                new Uri("http://localhost:52305/")
#else
                new Uri("https://oqpymanager.azurewebsites.net/")
#endif
                );
            try
            {
                return await api.ApiVenuesFilterPostAsync(GetVenue());
            }
            catch ( Exception ex )
            {
                throw;
            }
        }

        public static IForm<SearchVenues> Builder()
        {
            try
            {
                var build = new FormBuilder<SearchVenues>();
                        //.Field(nameof(Name));
                //.Field(nameof(Address));
                var a = build.Build();
                return a;
            }
            catch ( Exception ex )
            {
                throw;
            }
        }
    }
}