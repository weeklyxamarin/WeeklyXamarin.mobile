using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeeklyXamarin.AdminServices.Entities;
using WeeklyXamarin.AdminServices.Services;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditionController : ControllerBase
    {
        private readonly IUrlService urlService;
        private IEditionStorage EditionStorage;

        public EditionController(IUrlService urlService, IEditionStorage EditionStorage)
        {
            this.urlService = urlService;
            this.EditionStorage = EditionStorage;
        }

        // GET: api/<EditionController>
        [HttpGet]
        public async Task<IEnumerable<Edition>> Get()
        {
            var Editions = await EditionStorage.GetEditions();
            return Editions;
        }

        // GET api/<EditionController>/5
        [HttpGet("{id}")]
        public async Task<Edition> Get(string id)
        {
            var Edition = await EditionStorage.GetEdition(id);
            return Edition;
        }


        [HttpPost]
        public async Task<Edition> Post([FromBody] EditionEntity Edition)
        {
            await EditionStorage.PostEdition(Edition);
            return Edition;
        }


        // PUT api/<EditionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EditionController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            EditionStorage.DeleteEdition(id);
        }
    }
}
