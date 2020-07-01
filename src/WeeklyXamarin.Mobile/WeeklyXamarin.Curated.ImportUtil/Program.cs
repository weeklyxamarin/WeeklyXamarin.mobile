using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.ViewModels;

namespace WeeklyXamarin.Curated.ImportUtil
{
    class Program
    {
        static List<Edition> CuratedEditions = new List<Edition>();
        static List<Author> Authors = new List<Author>();

        static void Main(string[] args)
        {
            string directory = "C:\\curated\\data-export\\issues\\published";

            CuratedEditions = LoadupCuratedEditions(directory);

            Core.Models.Index index = CreateIndexFile(CuratedEditions);

            string indexJson = JsonConvert.SerializeObject(index,
                            Formatting.Indented,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            // pull out all the authors
            foreach (var edition in CuratedEditions)
            {
                foreach (var category in edition.Categories)
                {
                    foreach (var article in category.Items)
                    {
                        // check if the type is link

                        Article newArticle = new Article();
                        newArticle.Category = category.Name;
                        newArticle.Url = article.Url.ToString();
                        newArticle.Title = article.Title;
                        newArticle.Description = article.Description;
                        newArticle.EditionId = edition.Number.ToString();
                        
                        // try and find the author
                        if (article?.EmbeddedLinks?.Count > 1)
                        {
                            foreach (var link in article.EmbeddedLinks)
                            {
                                // ignore a link to the actual article
                                if (link.Url.ToString() == article.Url.ToString())
                                    continue;

                                // try and find an author with the link url
                                Author author = FindOrCreateAuthor(link.Url)

                                var author = new Author();
                                author.Name = link.Title;
                                author.Id = link.Url.ToString();
                                

                            }
                        }
                    }
                }
            }


            //foreach (var item in collection)
            //{

            //}


        }

        private static Author FindOrCreateAuthor(string url)
        {
            // see if we can find an auther with the id
            var author = Authors.FirstOrDefault(a => a.Id.ToLower() == url.ToLower());

            // see if we can find an author with the alias
            author =            

            if (author == null)
            {
                foreach (var auth in Authors)
                {
                    foreach (var alias in auth.Aliases)
                    {
                        if (alias.Name == url)
                        {
                            return author;
                        }
                    }
                }
            }
        }

        private static string AuthorUrlType(string url)
        {
            Uri uri = new Uri(url);
            switch (uri.Host)
            {
                case "twitter":
                    return "twitter";
                case "github":
                    return "github";
                default:
                    return "blog";
            }    
        }

        private static Core.Models.Index CreateIndexFile(List<Edition> curatedEditions)
        {
            // create an index file
            var indexData = new WeeklyXamarin.Core.Models.Index();
            indexData.UpdatedTimeStamp = DateTime.UtcNow;

            indexData.Editions = new List<Core.Models.Edition>();
            foreach (var edition in CuratedEditions)
            {
                var ed = edition.ToCoreEdition();
                indexData.Editions.Add(ed);
            }
            return indexData;
        }

        private static List<Edition> LoadupCuratedEditions(string directory)
        {
            string[] files = Directory.GetFiles(directory, "*.json", SearchOption.AllDirectories);
            //files = files.OrderByDescending(o => Convert.ToInt32(o)).ToArray();
            foreach (var item in files)
            {
                var json = File.ReadAllText(Path.Combine(directory, item));
                var edition = Curated.Edition.FromJson(json);
                CuratedEditions.Add(edition);
            }

            return CuratedEditions.OrderByDescending(o => o.Number).ToList();
        }
    }
}
