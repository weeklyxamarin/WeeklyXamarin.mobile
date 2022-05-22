using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WeeklyXamarin.AdminServices.Models;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;
using YoutubeExplode;

namespace WeeklyXamarin.AdminServices.Services
{
    public class UrlService : IUrlService
    {
        private IDataStore _dataStore;
        private IArticleStorage _articleStore;

        public UrlService(IDataStore dataStore, IArticleStorage articleStore)
        {
            _dataStore = dataStore;
            _articleStore = articleStore;
        }

        public async Task<Article> GetArticleDetailsFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));

            // check if article already in Github
            var article = await _dataStore.GetArticleForUrl(url);
            if (article != null) return article;

            // check if article in TableStorage
            var storageArticles = await _articleStore.SearchArticle(url);
            if (storageArticles.Any()) return storageArticles.FirstOrDefault();

            MetaInformation meta;

            // if youtube
            Uri articleUri = new Uri(url);
            if (string.Equals(articleUri.Host, "www.youtube.com", StringComparison.OrdinalIgnoreCase))
            {
                // get meta from youtubeexploder
                meta = await GetMetaDataFromYouTubeUrl(url);
            }
            else
            {
                meta = await GetMetaDataFromUrl(url);
            }

            List<string> bucketOfTokens = new List<string>();
            if (meta.Author != null) bucketOfTokens.Add(meta.Author);
            if (meta.AuthorTwitter != null) bucketOfTokens.Add(meta.AuthorTwitter);

            var author = await _dataStore.SearchAuthorsAsync(bucketOfTokens);
            if (author == null)
                author = await _dataStore.SearchAuthorsUrlAsync(url);

            // construct an article
            article = new Article
            {
                Url = url,
                Title = HttpUtility.HtmlDecode(meta?.Title),
                Description = HttpUtility.HtmlDecode(meta?.Description),
                Author = author?.Name,
            };
            return article;
        }

        private async Task<MetaInformation> GetMetaDataFromYouTubeUrl(string url)
        {
            //var youtubeVideo = await GetYoutubeChannelFromVideo(url);
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(url);

            var meta = new MetaInformation(url)
            {
                Title = video?.Title,
                Description = video?.Description,
                Author = video?.Author?.Title
            };
            return meta;
        }

        /// <summary>
        /// Uses HtmlAgilityPack to get the meta information from a url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<MetaInformation> GetMetaDataFromUrl(string url)
        {
            // Get the URL specified
            var webGet = new HtmlWeb();
            var document = await webGet.LoadFromWebAsync(url);
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
                                if (tagContent.Value.StartsWith("@"))
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
