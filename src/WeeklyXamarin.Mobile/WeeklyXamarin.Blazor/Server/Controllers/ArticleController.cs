using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeeklyXamarin.AdminServices.Entities;
using WeeklyXamarin.AdminServices.Services;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Models.Api;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeeklyXamarin.Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IUrlService urlService;
        private IArticleStorage articleStorage;

        public ArticleController(IUrlService urlService, IArticleStorage articleStorage)
        {
            this.urlService = urlService;
            this.articleStorage = articleStorage;
        }

        // GET: api/<ArticleController>
        [HttpGet]
        public async Task<IEnumerable<Article>> Get()
        {
            var articles = await articleStorage.GetArticles();
            return articles;
        }

        // GET api/<ArticleController>/5
        [HttpGet("{id}")]
        public async Task<Article> Get(string id)
        {
            var article = await articleStorage.GetArticle(id);
            return article; 
        }

        [Route("Lookup")]
        // POST api/<ArticleController>
        [HttpPost]
        public async Task<Article> GetArticleFromUrl([FromBody] LookupUrlRequest urlRequest)
        {
            ArgumentNullException.ThrowIfNull(urlRequest.Url);

            var a = await urlService.GetArticleDetailsFromUrl(urlRequest.Url);

            return a;
        }

        [HttpPost]
        public async Task<Article> Post([FromBody] ArticleEntity article)
        {
            await articleStorage.PostArticle(article);
            return article;
        }


        // PUT api/<ArticleController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ArticleController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var success = await articleStorage.DeleteArticle(id);

            if(success)
                return NoContent();
            else 
                return BadRequest($"{id} could not be deleted");
        }
    }
}
