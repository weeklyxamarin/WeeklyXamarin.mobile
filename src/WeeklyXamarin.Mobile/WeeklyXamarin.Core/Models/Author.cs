﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeeklyXamarin.Core.Models
{
    public class Author
    {
        public string Name { get; set; }
        public string TwitterHandle { get; set; }
        [JsonIgnore]
        public string TwitterUrl => $"https://twitter.com/{TwitterHandle}";
        public string GitHubHandle { get; set; }
        [JsonIgnore]
        public string GitHubUrl => $"https://github.com/{GitHubHandle}";
        public string TwitchHandle { get; set; }
        [JsonIgnore]
        public string TwitchUrl => $"https://twitch.tv/{TwitchHandle}";
        public string YouTubeUrl { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Id { get; set; }
        public List<Alias> Aliases { get; set; }
        public string Website { get; set; }
    }
}
