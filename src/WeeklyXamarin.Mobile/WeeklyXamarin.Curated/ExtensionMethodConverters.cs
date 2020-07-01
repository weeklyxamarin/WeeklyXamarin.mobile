using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace WeeklyXamarin.Curated
{
    public static class ExtensionMethodConverters
    {
        public static Core.Models.Edition ToCoreEdition(this Edition edition)
        {
            Core.Models.Edition ed = new Core.Models.Edition();
            ed.PublishDate = edition.PublishedAt.UtcDateTime;
            ed.Summary = edition.Summary?.ToString();
            ed.Name = edition?.Title;
            ed.Id = edition.Number.ToString();
            ed.Curators = "";
            return ed;
        }
    }
}
