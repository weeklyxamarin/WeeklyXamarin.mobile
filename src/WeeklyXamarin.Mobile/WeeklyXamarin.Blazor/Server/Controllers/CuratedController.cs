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

        private readonly ICuratedService urlService;
        private readonly IConfiguration config;

        public CuratedController(ICuratedService urlService, IConfiguration config)
        {
            this.urlService = urlService;
            this.config = config;

        }

        // POST api/<ArticleController>
        [HttpPost]
        public async Task<string> PostArticleToCurated([FromBody] Article article)
        {
            ArgumentNullException.ThrowIfNull(article);

            var apiKey = config["curatedApiKey"];
            var subscription = config["subscriptionId"];

            urlService.ApiKey = apiKey;
            urlService.Subscription = subscription;

            var a = await urlService.PostArticleToCurated(article);
            return a;
        }
    }
}
