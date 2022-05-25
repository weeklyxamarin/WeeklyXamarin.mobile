using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.AdminServices.Entities;

namespace WeeklyXamarin.AdminServices.Services
{
    public class EditionStorage : IEditionStorage
    {

        private ITableService<EditionEntity> tableService;

        public EditionStorage(ITableService<EditionEntity> tableService)
        {
            this.tableService = tableService;
        }

        public async Task<bool> DeleteEdition(string id)
        {
            return await tableService.DeleteAsync(id);
        }

        public async Task<EditionEntity> GetEdition(string id)
        {
            var Edition = await tableService.GetAsync(id);
            return Edition;
        }

        public async Task<List<EditionEntity>> GetEditions()
        {
            // TODO: Get the list of Editions from table storage
            var Editions = await tableService.GetAllAsync();
            return Editions;
        }

        public async Task<EditionEntity> PostEdition(EditionEntity edition)
        {
            return await tableService.SaveAsync(edition);
        }

        
    }
}
