using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using WeeklyXamarin.AdminServices.Services;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Models.Api;

namespace WeeklyXamarin.Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuratedController : ControllerBase
    {

        private readonly ICuratedService curatedService;
        private readonly IConfiguration config;

        public CuratedController(ICuratedService curatedService, IConfiguration config)
        {
            this.curatedService = curatedService;
            this.config = config;
        }

        [HttpPost]
        public async Task<string> PostArticleToCurated([FromBody] Article article)
        {
            ArgumentNullException.ThrowIfNull(article);

            // get keys from environment variables
            var apiKey = config["curatedApiKey"];
            var subscription = config["subscriptionId"];

            curatedService.ApiKey = apiKey;
            curatedService.Subscription = subscription;

            var responseSTring = await curatedService.PostArticleToCurated(article);
            return responseSTring;
        }
    }
}
