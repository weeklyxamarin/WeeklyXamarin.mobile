using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeeklyXamarin.Core.Helpers;

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

        [JsonIgnore] // we don't want to persist this
        public string PreferredContact { 
            get
            {
                if (!string.IsNullOrEmpty(TwitterHandle))
                    return TwitterUrl;
                if (!string.IsNullOrEmpty(GitHubHandle))
                    return GitHubUrl;
                if (!string.IsNullOrEmpty(YouTubeUrl))
                    return YouTubeUrl;
                if (!string.IsNullOrEmpty(TwitchHandle))
                    return TwitchUrl;
                if (!string.IsNullOrEmpty(Website))
                    return Website;
                return "";
            }
        }

        internal bool Matches(List<string> tokens)
            => tokens.Any(x => Name.EqualsCaseInsensitive(x) 
                            || TwitchHandle.EqualsCaseInsensitive(x)
                            || TwitterHandle.EqualsCaseInsensitive(x) 
                            || GitHubHandle.EqualsCaseInsensitive(x));

        internal bool MatchesUrl(string url)
        {
            if (Website == null) return false;

            return url.Contains(Website, StringComparison.InvariantCultureIgnoreCase); 
        }
    }

}
