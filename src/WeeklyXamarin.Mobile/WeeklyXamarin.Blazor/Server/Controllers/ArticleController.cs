using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public ArticleController(IUrlService urlService)
        {
            this.urlService = urlService;
        }

        // GET: api/<ArticleController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ArticleController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ArticleController>
        [HttpPost]
        public async Task<Article> GetArticleFromUrl([FromBody] LookupUrlRequest urlRequest)
        {
            var a = await urlService.GetArticleDetailsFromUrl(urlRequest.Url);
            return a;

            //var article = new Article();
            //article.Title = "Article From Controller";
            //article.Url = urlRequest.Url;
            //article.Author = "R2";
            //return article;
        }


        //// POST api/<ArticleController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<ArticleController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ArticleController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
