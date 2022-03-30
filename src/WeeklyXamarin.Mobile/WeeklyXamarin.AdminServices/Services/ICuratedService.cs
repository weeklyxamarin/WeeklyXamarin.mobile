using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.AdminServices.Entities;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Models.Api;

namespace WeeklyXamarin.AdminServices.Services
{

    public interface ICuratedService
    {
        string ApiKey { get; set; }

        string Subscription { get; set; }

        Task<string> PostArticle(Article article);
    }
}
