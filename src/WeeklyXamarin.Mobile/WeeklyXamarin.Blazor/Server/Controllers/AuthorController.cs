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
    public class AuthorController : ControllerBase
    {
        private readonly IUrlService urlService;
        private IAuthorStorage AuthorStorage;

        public AuthorController(IUrlService urlService, IAuthorStorage AuthorStorage)
        {
            this.urlService = urlService;
            this.AuthorStorage = AuthorStorage;
        }

        // GET: api/<AuthorController>
        [HttpGet]
        public async Task<IEnumerable<Author>> Get()
        {
            var Authors = await AuthorStorage.GetAuthors();
            return Authors;
        }

        // GET api/<AuthorController>/5
        [HttpGet("{id}")]
        public async Task<Author> Get(string id)
        {
            var Author = await AuthorStorage.GetAuthor(id);
            return Author; 
        }


        [HttpPost]
        public async Task<Author> Post([FromBody] AuthorEntity Author)
        {
            await AuthorStorage.PostAuthor(Author);
            return Author;
        }


        // PUT api/<AuthorController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthorController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            AuthorStorage.DeleteAuthor(id);
        }
    }
}
