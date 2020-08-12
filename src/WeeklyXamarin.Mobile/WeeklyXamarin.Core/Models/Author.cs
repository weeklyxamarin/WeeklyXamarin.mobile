using System;
using System.Collections.Generic;
using System.Text;

namespace WeeklyXamarin.Core.Models
{
    public class Author
    {
        public string Name { get; set; }
        public string TwitterHandle { get; set; }
        public string GitHubHandle { get; set; }
        public string TwitchHandle { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Id { get; set; }
        public List<Alias> Aliases { get; set; }
        public string Website { get; set; }
    }
}
