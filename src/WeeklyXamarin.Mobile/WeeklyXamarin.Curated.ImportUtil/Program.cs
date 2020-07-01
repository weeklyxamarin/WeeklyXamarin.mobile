using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Responses;
using WeeklyXamarin.Core.ViewModels;


namespace WeeklyXamarin.Curated.ImportUtil
{
    class Program
    {
        static List<Edition> CuratedEditions = new List<Edition>();
        //static List<Author> Authors = new List<Author>();
        static AuthorResponse AuthorLookup;
        static Core.Models.Index indexLookup;
        

        static string curatedExportPath;
        static string basePath;
        static string outputPath = "Output";
        static string lookupDataPath;
        const string AuthorsFile = "authors.json";
        const string IndexFile = "index.json";

        static void Main(string[] args)
        {
            string executeLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            basePath = System.IO.Path.GetDirectoryName(executeLocation);

            outputPath = Path.Combine(basePath, "Output");
            curatedExportPath = Path.Combine(basePath, "CuratedExport");
            lookupDataPath = Path.Combine(basePath, "LookupData");

            // load up the authors
            Program.AuthorLookup = LoadAuthorLookup(Path.Combine(lookupDataPath, AuthorsFile));

            CuratedEditions = LoadupCuratedEditions(curatedExportPath);

            // create the index file
            Core.Models.Index indexLookup = CreateIndex(CuratedEditions);
            WriteIndexFile(Path.Combine(outputPath, IndexFile), indexLookup);


            // pull out all the editions
            foreach (var curatedEdition in CuratedEditions)
            {
                Core.Models.Edition newEdition = new Core.Models.Edition();
                newEdition = curatedEdition.ToCoreEdition();

                foreach (var category in curatedEdition.Categories)
                {
                    foreach (var article in category.Items)
                    {
                        // only process link types
                        if (article.Type == TypeEnum.Text) continue;

                        Article newArticle = new Article();
                        newArticle.Category = category.Name;
                        newArticle.Url = article.Url.ToString();
                        newArticle.Title = article.Title;
                        newArticle.Description = article.Description;
                        newArticle.EditionId = curatedEdition.Number.ToString();
                        
                        // try and find the author
                        if (article?.EmbeddedLinks?.Count > 1)
                        {
                            foreach (var link in article.EmbeddedLinks)
                            {
                                // ignore a link to the actual article
                                if (link.Url.ToString() == article.Url.ToString())
                                    continue;

                                // try and find an author with the link url
                                Author author = FindOrCreateAuthor(link.Url.ToString(), link.Title);
                                newArticle.Author = author.Id;
                            }
                        }

                        newEdition.Articles.Add(newArticle);


                    }
                }

                WriteEditionFile(Path.Combine(outputPath, $"{newEdition.Id}.json"), newEdition);

            }

            WriteAuthorsFile(Path.Combine(outputPath, AuthorsFile), AuthorLookup);


        }

        private static void WriteEditionFile(string filename, Core.Models.Edition newEdition)
        {
            string editionJson = JsonConvert.SerializeObject(newEdition,
        Formatting.Indented,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
            File.WriteAllText(filename, editionJson);
        }

        private static void WriteAuthorsFile(string filename, AuthorResponse authorLookup)
        {
            AuthorResponse outpuAuthors = new AuthorResponse();
            outpuAuthors.Authors = authorLookup.Authors.OrderBy(a => a.Name).ToList();

            string authorJson = JsonConvert.SerializeObject(outpuAuthors,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
            File.WriteAllText(filename, authorJson);
        }

        private static void WriteIndexFile(string filename, Core.Models.Index index)
        {
            string indexJson = JsonConvert.SerializeObject(index,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            File.WriteAllText(filename, indexJson);
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

        private static Author FindOrCreateAuthor(string authorKey, string authorDisplayName)
        {
            Author author;
            
            // see if we can find an auther with the id
            author = AuthorLookup.Authors.FirstOrDefault(a => a.Id.ToLower() == authorKey.ToLower());
            
            if (author != null)
            {
                System.Diagnostics.Debug.WriteLine($"Found Existing Author by key: {authorKey}");
                return author;
            }

            // now we need to see if we can find an author by alias matching
            foreach (var auth in AuthorLookup.Authors)
            {
                foreach (var alias in auth.Aliases)
                {
                    if (alias.Name.ToLower() == authorKey.ToLower())
                    {
                        System.Diagnostics.Debug.WriteLine($"Found Existing Author by alias: {authorKey}");
                        return auth;
                    }
                }
            }

            // try and find author by actual name (just for warning output)
            author = AuthorLookup.Authors.FirstOrDefault(a => a.Name.ToLower() == authorDisplayName.ToLower());

            if (author != null)
            {
                System.Diagnostics.Debug.WriteLine($"Found an author by name match, so you'll want to add an alias - Name: {authorDisplayName}, {authorKey}");
            }


            // at this point we can't find an author so we need to create a new one
            author = new Author();
            author.Id = authorKey.ToLower();
            author.Name = authorDisplayName;
            author.Aliases = new List<Alias>();
            var newAlias = new Alias();
            newAlias.Name = authorKey.ToLower();
            newAlias.Type = AuthorUrlType(authorKey.ToLower());
            author.Aliases.Add(newAlias);

            System.Diagnostics.Debug.WriteLine($"Creating a new author object - Name: {authorDisplayName}, {authorKey}");

            AuthorLookup.Authors.Add(author);

            return author;
        }

        private static string AuthorUrlType(string url)
        {
            Uri uri = new Uri(url);
            switch (uri.Host)
            {
                case "twitter.com":
                    return "twitter";
                case "github.com":
                    return "github";
                default:
                    return "blog";
            }    
        }

        private static Core.Models.Index CreateIndex(List<Edition> curatedEditions)
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
