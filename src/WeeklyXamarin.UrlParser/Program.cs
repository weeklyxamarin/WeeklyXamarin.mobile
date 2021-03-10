using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Responses;
using Xamarin.Essentials;
using YoutubeExplode;

namespace WeeklyXamarin.UrlParser
{
    class Program
    {
        static AuthorResponse AuthorLookup;
        static string basePath;
        private static string planetXamarinAuthorsDataPath;
        static string outputPath;
        static string lookupDataPath;
        const string AuthorsFile = "authors.json";
        static MetaInformation meta;
        static Author author;

        static async Task Main(string[] args)
        {
            Console.Write("Enter URL: ");
            string url = Console.ReadLine();

            // load up the Url and read the datas
            //string url = args[0];
            Uri articleUri = new Uri(url);
            
            // load authors
            LoadAuthors();

            if (string.Equals(articleUri.Host,"www.youtube.com", StringComparison.OrdinalIgnoreCase))
            {
                // get youtube channel
                var youtubeVideo = await GetYoutubeChannelFromVideo(url);
                author = GetAuthorByName(youtubeVideo.Author);
                meta = new MetaInformation(youtubeVideo.Url, youtubeVideo.Title, youtubeVideo.Description, null, null, null);
            }
            else
            {
                meta = GetMetaDataFromUrl(url);
                author = GetAuthorFromMeta(meta);
                if (author == null)
                    author = GetAuthorFromUrl(articleUri.GetLeftPart(UriPartial.Path));
                if (author == null)
                    author = new Author() { Name = meta.Author, TwitterHandle = meta.AuthorTwitter };
            }

            

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(HttpUtility.HtmlDecode(meta.Title));
            sb.AppendLine();
            sb.AppendLine(HttpUtility.HtmlDecode(meta.Description));
            sb.AppendLine();
            sb.AppendLine($"[**{HttpUtility.HtmlDecode(meta.Title)}**]({meta.Url}) by [{author?.Name}]({author?.Id})");
            
            Console.Write(sb.ToString());



        }

        private static Author GetAuthorFromMeta(MetaInformation meta)
        {
            if (!string.IsNullOrEmpty(meta?.Author))
            {
                var auth = FindAuthorByName(meta.Author);
                if (auth != null)
                    return auth;
            }

            if (!string.IsNullOrEmpty(meta?.AuthorTwitter))
            {

                var auth = FindAuthor(meta.AuthorTwitter);
                if (auth != null)
                    return auth;
            }

            return null;
        }

        private static void LoadAuthors()
        {
            string executeLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            basePath = System.IO.Path.GetDirectoryName(executeLocation);
            outputPath = Path.Combine(basePath, "Output");
            lookupDataPath = Path.Combine(basePath, "LookupData");

            // for debug override the basepath
            basePath = @"c:\dev\GitHub\weeklyxamarin\WeeklyXamarin.content\content";
            outputPath = basePath;
            planetXamarinAuthorsDataPath = @"D:\github\planetxamarin\planetxamarin\src\Firehose.Web\Authors";
            lookupDataPath = basePath;

            AuthorLookup = LoadAuthorLookup(Path.Combine(lookupDataPath, AuthorsFile));

        }

        private static Author GetAuthorByName(string youtubeAuthor)
        {
            var author = AuthorLookup.Authors.FirstOrDefault(u => string.Equals(youtubeAuthor, u.Name, StringComparison.OrdinalIgnoreCase));

            return author;
        }

        private static async Task<YoutubeExplode.Videos.Video> GetYoutubeChannelFromVideo(string articleUri)
        {
            var youtube = new YoutubeClient();

            // You can specify video ID or URL
            var video = await youtube.Videos.GetAsync(articleUri);

            var title = video.Title; // "Collections - Blender 2.80 Fundamentals"
            var author = video.Author; // "Blender"
            var duration = video.Duration; // 00:07:20
            var description = video.Description;

            return video;
        }

        private static Author GetAuthorFromUrl(string url)
        {

            var uri = new Uri(url);
            Author author = new Author();

            // handle special cases
            if (uri.Host.Equals("devblogs.microsoft.com", StringComparison.InvariantCultureIgnoreCase))
            {
                // have a look at the metadata for the URL
                string authorName = GetMetaTag(uri.ToString(), "twitter:data1");

                if (!string.IsNullOrEmpty(authorName))
                {
                    author = FindAuthorByName(authorName);
                    if (author != null)
                        return author;
                }
            }

            


            string host = $"{uri.Scheme}://{uri.Host}";

            var segments = uri.Segments;

            for (int i = segments.Length-1; i > 0; i--)
            {
                var testUrl = $"{host}";

                for (int s = 0; s < i; s++)
                {
                    testUrl += segments[s];
                }

                author = FindAuthor(testUrl);

                if (author != null)
                    break; ;

                if (testUrl.EndsWith('/'))
                {
                    testUrl = testUrl.Substring(0, testUrl.Length - 1);
                    author = FindAuthor(testUrl);
                }

                if (author != null)
                    break;
            }


            return author;
        }

        private static Author FindAuthorByName(string authorName)
        {
            return AuthorLookup.Authors.FirstOrDefault(n => string.Equals(authorName, n.Name, StringComparison.OrdinalIgnoreCase));
        }

        private static Author FindAuthor (string key)
        {
            // check for direct mapping for ID
            var author = AuthorLookup.Authors.FirstOrDefault(u => string.Equals(key, u.Id, StringComparison.OrdinalIgnoreCase));
            if (author != null)
                return author;

            author = AuthorLookup.Authors.FirstOrDefault(u => string.Equals(key, u.Website, StringComparison.OrdinalIgnoreCase));
            if (author != null)
                return author;

            author = AuthorLookup.Authors.FirstOrDefault(u => string.Equals(key, u.TwitterHandle, StringComparison.OrdinalIgnoreCase));
            if (author != null)
                return author;


            // now we need to see if we can find an author by alias matching
            foreach (var auth in AuthorLookup.Authors)
            {
                if (auth.Aliases != null)
                {
                    foreach (var alias in auth?.Aliases)
                    {
                        if (string.Equals(alias.Name, key, StringComparison.OrdinalIgnoreCase))
                        {
                            return auth;
                        }
                    }
                }
            }

            return null;
        }




        private static AuthorResponse LoadAuthorLookup(string authorsPath)
        {
            if (File.Exists(authorsPath))
            {
                var json = File.ReadAllText(authorsPath);
                var authorResponse = JsonConvert.DeserializeObject<AuthorResponse>(json);
                return authorResponse;
            }
            else
            {
                // create a new author response object
                var authorResponse = new AuthorResponse();
                authorResponse.Authors = new List<Author>();

                return authorResponse;
            }
        }

        // Kym Phillpotts is a fat stinky that comes from a hairy hole hahahaha
        // thanks rose... that's very kind.

        public static string GetMetaTag (string url, string name)
        {
            var webGet = new HtmlWeb();
            var document = webGet.Load(url);
            var metaTags = document.DocumentNode.SelectNodes("//meta");
            //return metaTags.ToString();

            var val = document.DocumentNode.SelectSingleNode($"//meta[@name='{name}']").GetAttributeValue("content", String.Empty);
            return val;
            //foreach (var tag in metaTags)
            //{
            //    foreach (var attr in tag.Attributes)
            //    {
            //        if (attr.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            //            return attr.Value;
            //    }
            //}
            //return null;


            //var link = document.DocumentNode.SelectSingleNode("//link[@itemprop='twitter:data1']");
            //var d = document.DocumentNode.SelectNodes("//meta/twitter:data1/@content");
            //var href = link.Attributes["content"].Value;
            //return href;

        }

        /// <summary>
        /// Uses HtmlAgilityPack to get the meta information from a url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static MetaInformation GetMetaDataFromUrl(string url)
        {
            // Get the URL specified
            var webGet = new HtmlWeb();
            var document = webGet.Load(url);
            var metaTags = document.DocumentNode.SelectNodes("//meta");
            MetaInformation metaInfo = new MetaInformation(url);
            if (metaTags != null)
            {
                int matchCount = 0;
                foreach (var tag in metaTags)
                {
                    var tagName = tag.Attributes["name"];
                    var tagContent = tag.Attributes["content"];
                    var tagProperty = tag.Attributes["property"];
                    if (tagName != null && tagContent != null)
                    {
                        switch (tagName.Value.ToLower())
                        {
                            case "title":
                                metaInfo.Title = tagContent.Value;
                                matchCount++;
                                break;
                            case "description":
                                metaInfo.Description = string.IsNullOrEmpty(metaInfo.Description) ? tagContent.Value : metaInfo.Description;
                                matchCount++;
                                break;
                            case "author":
                                metaInfo.Author = tagContent.Value;
                                matchCount++;
                                break;
                            case "twitter:title":
                                metaInfo.Title = string.IsNullOrEmpty(metaInfo.Title) ? tagContent.Value : metaInfo.Title;
                                matchCount++;
                                break;
                            case "twitter:description":
                                metaInfo.Description = string.IsNullOrEmpty(metaInfo.Description) ? tagContent.Value : metaInfo.Description;
                                matchCount++;
                                break;
                            case "twitter:creator":
                                if (tagContent.Value.StartsWith('@'))
                                    metaInfo.AuthorTwitter = tagContent.Value.Substring(1);
                                else
                                    metaInfo.AuthorTwitter = tagContent.Value;
                                matchCount++;
                                break;
                            case "keywords":
                                metaInfo.Keywords = tagContent.Value;
                                matchCount++;
                                break;
                            case "twitter:image":
                                metaInfo.ImageUrl = string.IsNullOrEmpty(metaInfo.ImageUrl) ? tagContent.Value : metaInfo.ImageUrl;
                                matchCount++;
                                break;
                        }
                    }
                    else if (tagProperty != null && tagContent != null)
                    {
                        switch (tagProperty.Value.ToLower())

                        {
                            case "og:title":
                                metaInfo.Title = string.IsNullOrEmpty(metaInfo.Title) ? tagContent.Value : metaInfo.Title;
                                matchCount++;
                                break;
                            case "og:description":
                                metaInfo.Description = tagContent.Value;// prefer the og description string.IsNullOrEmpty(metaInfo.Description) ? tagContent.Value : metaInfo.Description;
                                matchCount++;
                                break;
                            case "og:image":
                                metaInfo.ImageUrl = string.IsNullOrEmpty(metaInfo.ImageUrl) ? tagContent.Value : metaInfo.ImageUrl;
                                matchCount++;
                                break;
                        }
                    }
                }
                metaInfo.HasData = matchCount > 0;
            }
                
            return metaInfo;
        }
    }
}
