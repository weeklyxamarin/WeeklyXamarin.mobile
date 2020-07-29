using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.Curated
{
    public static class ExtensionMethodConverters
    {
        public static Core.Models.Edition ToCoreEdition(this Edition edition)
        {
            Core.Models.Edition ed = new Core.Models.Edition();
            ed.PublishDate = edition.PublishedAt.UtcDateTime;
            ed.UpdatedTimeStamp = edition.UpdatedAt.UtcDateTime;

            if (String.IsNullOrEmpty(edition.Summary?.ToString()))
                ed.Summary = "No Edition Summary";
            else
                ed.Summary = edition.Summary?.ToString();

            ed.Name = edition?.Title;
            ed.Id = edition.Number.ToString();
            ed.Curators = "";
            ed.Articles = new List<Article>();

            return ed;
        }
    }
}
