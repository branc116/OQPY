using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OQPYManager.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using OQPYManager.Models;
using Microsoft.AspNetCore.Authorization;
using OQPYManager.Models.CoreModels;
using OQPYManager.Data;
using Microsoft.Extensions.Options;

namespace OQPYManager.Controllers
{
    [Produces("application/json")]
    [Route("api/V0")]
    public class RestfulOqpyController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly string _externalCookieScheme;
        private readonly VenuesDbContext _venusDb;
        public RestfulOqpyController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<IdentityCookieOptions> identityCookieOptions,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory,
            VenuesDbContext venusDb

            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _venusDb = venusDb;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Venue>> FindVenues(string Type, string Name, string Location , string GeneralSearch, string SortBy, bool Lossy = true, int NumberOfResults = 40)
        {
            var ret = (from _ in _venusDb.Venues
                       where string.IsNullOrEmpty(Type) ? true : _.Tags.Any((i) => i.TagName == Type)
                       orderby SortBy ?? "Name"
                       select _).Take(NumberOfResults);
            return ret;
        }

    }
}