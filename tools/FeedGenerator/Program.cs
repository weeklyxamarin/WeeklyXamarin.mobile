using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.Linq.Expressions;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WeeklyXamarin.Core.Models;

namespace FeedGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            const string baseUrl = @"https://raw.githubusercontent.com/weeklyxamarin/WeeklyXamarin.content/master/content/";
            const string indexFile = "index.json";

            // get from the interwebs
            HttpClient _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseUrl);

            var indexResponse = await _httpClient.GetStringAsync(indexFile);
            var index = JsonConvert.DeserializeObject<WeeklyXamarin.Core.Models.Index>(indexResponse);

            // now we got index.
            foreach (var item in index.Editions)
            {
                var editionFile = $"{item.Id}.json";
                var editionResponse = await _httpClient.GetStringAsync(editionFile);
                var edition = JsonConvert.DeserializeObject<Edition>(editionResponse);

                StringBuilder sb = new StringBuilder();
                sb.Append("<?xml version='1.0' encoding='utf8'?>< opml version = \"2.0\" >< head >< title > Feeds </ title > </ head >    < body > ");

                foreach (var article in edition.Articles)
                {
                    Uri uri = new Uri(article.Url);
                    Console.WriteLine($"{uri.Scheme}://{uri.Host}");

                    string feedUrl = $"https://feedsearch.dev/api/v1/search?opml=true&url={article.Url.ToString()}";
                    string result;
                    try
                    {
                        result = await _httpClient.GetStringAsync(feedUrl);
                    }
                    catch (Exception ex)
                    {
                        result = "<body />";
                    }

                    if (result.Contains("<body />"))
                    {
                        // we can't find a feed
                        Console.WriteLine("  CAN'T FIND FEED FOR: " + uri.Host);
                    }
                    else
                    {
                        // strip everyting before <body>
                        var start = result.IndexOf("<body>");
                        var end = result.IndexOf("</body>");
                        var startPos = start + 6;
                        var length = (end - 6) - start;
                        string content = result.Substring(startPos, length);
                        sb.AppendLine(content);
                    }

                    

                    //string feedUrl = DiscoverFeedUrl(uri);

                }

                sb.AppendLine("    </body></ opml > ");
                Console.WriteLine(sb.ToString());
            }



        }

        private static string DiscoverFeedUrl(Uri uri)
        {
            string feedUriToTest;
            if (uri.Host == "")
                feedUriToTest = $"{uri.Scheme}://{uri.Host}/feed";
            feedUriToTest = uri.ToString();
            if (TryParseFeed(feedUriToTest))
                return feedUriToTest;
            else return "";
        }

            public static bool TryParseFeed(string url)
            {
                try
                {
                    SyndicationFeed feed = SyndicationFeed.Load(XmlReader.Create(url));
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
        }
    }
}
