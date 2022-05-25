using System.Collections.Generic;
using System.Threading.Tasks;
using WeeklyXamarin.AdminServices.Entities;

namespace WeeklyXamarin.AdminServices.Services
{
    public interface IEditionStorage
    {
        Task<bool> DeleteEdition(string id);
        Task<EditionEntity> GetEdition(string id);
        Task<List<EditionEntity>> GetEditions();
        Task<EditionEntity> PostEdition(EditionEntity edition);
    }
}