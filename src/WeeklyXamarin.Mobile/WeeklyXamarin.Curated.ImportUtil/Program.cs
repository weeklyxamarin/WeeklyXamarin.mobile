﻿using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Responses;
using Xamarin.Essentials;

namespace WeeklyXamarin.Curated.ImportUtil
{
    class Program
    {
        static List<Edition> CuratedEditions = new List<Edition>();
        static AuthorResponse AuthorLookup;
        static Core.Models.Index indexLookup;
        static List<Core.Models.Edition> Editions = new List<Core.Models.Edition>();

        static string basePath;
        static string curatedDataPath;
        private static string planetXamarinAuthorsDataPath;
        static string outputPath;
        static string lookupDataPath;
        const string AuthorsFile = "authors.json";
        const string IndexFile = "index.json";
        const string sponsoredCategory = "sponsored";

        static void Main(string[] args)
        {
            // work out our paths
            // TODO: These should probably by derived from args
            string executeLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            basePath = System.IO.Path.GetDirectoryName(executeLocation);
            outputPath = Path.Combine(basePath, "Output");
            curatedDataPath = Path.Combine(basePath, "CuratedExport");
            lookupDataPath = Path.Combine(basePath, "LookupData");

            // for debug override the basepath
            basePath = @"D:\GitHub\weeklyxamarin\WeeklyXamarin.content\content";
            outputPath = basePath;
            curatedDataPath = @"D:\GitHub\weeklyxamarin\WeeklyXamarin.content\curateddata\published";
            planetXamarinAuthorsDataPath = @"c:\github\planetxamarin\planetxamarin\src\Firehose.Web\Authors";
            lookupDataPath = basePath;

            // load up the editions from curated files
            CuratedEditions = LoadupCuratedEditions(curatedDataPath);

            // create the index file - before we have the articles
            Core.Models.Index indexLookup = LoadIndexFile(Path.Combine(lookupDataPath, IndexFile));
            UpdateIndexFile(indexLookup, CuratedEditions);
            WriteIndexFile(Path.Combine(outputPath, IndexFile), indexLookup);

            // load up the authors lookup file which is used
            // to try and identify authors via multiple means
            AuthorLookup = LoadAuthorLookup(Path.Combine(lookupDataPath, AuthorsFile));

            ProcessEditions();
            //ProcessPlanetXamarinAuthors();


            // finally we have an authors
            WriteAuthorsFile(Path.Combine(outputPath, AuthorsFile), AuthorLookup);

            OutputEditions(outputPath, Editions);

        }

        private static void ProcessPlanetXamarinAuthors()
        {
            // read all files in PlanetXamarinAuthors path
            string[] files = Directory.GetFiles(planetXamarinAuthorsDataPath, "*.cs", SearchOption.AllDirectories);
            foreach (var item in files)
            {
                var authorFileText = File.ReadAllText(Path.Combine(planetXamarinAuthorsDataPath, item));

                System.Diagnostics.Debug.WriteLine($"Parsing {item}");
                // find the token we are looking for
                var fn = GetTokenValue(authorFileText, "FirstName");
                var ln = GetTokenValue(authorFileText, "LastName");
                var name = $"{fn} {ln}".Trim();
                var th = GetTokenValue(authorFileText, "TwitterHandle");
                var gh = GetTokenValue(authorFileText, "GitHubHandle");
                var ws = GetTokenValue(authorFileText, "WebSite");
                
                var user = FindOrCreateAuthorByName(name);
                if (user != null)
                {
                    user.TwitterHandle = th;
                    user.GitHubHandle = gh;
                    user.Website = ws;
                    if (String.IsNullOrEmpty(user.Id))
                    {
                        if (!string.IsNullOrEmpty(user.TwitterHandle))
                        {
                            user.Id = $"https://twitter.com/{th}";
                        }
                        else if (!string.IsNullOrEmpty(user.GitHubHandle))
                        {
                            user.Id = $"https://github.com/{gh}";
                        }

                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Could not find user {name}");
                }
            }
        }

        private static string GetTokenValue(string authorFileText, string token)
        {
            //var fullToken = $"{token} => ";
            var index = authorFileText.IndexOf(token);
            if (index != -1)
            {
                var startIndex = index + token.Length;
                var endIndex = authorFileText.IndexOf(";", index);
                string lineText = authorFileText.Substring(index, endIndex - index).Trim();
                var valStart = lineText.IndexOf('"');
                if (valStart != -1)
                {
                    var valEnd = lineText.LastIndexOf('"');
                    var val = lineText.Substring(valStart+1, (valEnd-1) - valStart);
                    System.Diagnostics.Debug.WriteLine($"  {token}: {val}");
                    return val;
                }
                return "";

                //var val = authorFileText.Substring(startIndex, (endIndex-1) - startIndex);
                
                
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"*** UNABLE TO FIND {token}");
                return "";
            }
        }

        private static Core.Models.Index LoadIndexFile(string indexPath)
        {
            if (File.Exists(indexPath))
            {
                var json = File.ReadAllText(indexPath);
                var index = JsonConvert.DeserializeObject<Core.Models.Index>(json);
                return index;
            }
            else
                return new Core.Models.Index();
        }

        private static void OutputEditions(string outputPath, List<Core.Models.Edition> editions)
        {
            foreach (var edition in editions)
            {
                WriteEditionFile(Path.Combine(outputPath, $"{edition.Id}.json"), edition);
            }
        }

        private static void ProcessEditions()
        {
            // pull out all the editions
            foreach (var curatedEdition in CuratedEditions)
            {
                Core.Models.Edition newEdition = new Core.Models.Edition();
                newEdition = curatedEdition.ToCoreEdition();
                var articleCount = 0;
                // process all the categories
                foreach (var category in curatedEdition.Categories)
                {
                    // process each article for the category
                    foreach (var article in category.Items)
                    {
                        //if (category.Code == "")

                        // only process link types, ignore the text types
                        if (article.Type == TypeEnum.Text)
                        {
                            if (IsCuratorElementCategory(category.Code))
                            {
                                string introText = RemoveMarkdownFormatting(article.DescriptionMarkdown);
                                newEdition.Introduction = introText;
                                newEdition.Curators = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(category.Name.ToLower());
                                continue;
                            }
                            else
                                continue;
                        };

                        // check if this is a sponsored link
                        if (category.Code == sponsoredCategory)
                        {
                            // we should treat this a little different
                            continue;
                        }
                        

                        articleCount++;
                        Article newArticle = new Article();
                        newArticle.Category = category.Name;
                        newArticle.Url = article.Url.ToString();
                        newArticle.Title = article.Title;

                        newArticle.Description = RemoveMarkdownFormatting(article.DescriptionMarkdown);
                        //newArticle.Description = article.DescriptionMarkdown;
                        //newArticle.Description = RemoveHTMLFormatting(article.Description);
                        //newArticle.Description = article.Description;
                        newArticle.EditionId = curatedEdition.Number.ToString();
                        newArticle.Id = $"{newArticle.EditionId}-{articleCount}";

                        // try and find the author. We usually have these as links in the description
                        if (article?.EmbeddedLinks?.Count > 1)
                        {
                            foreach (var link in article.EmbeddedLinks)
                            {
                                // ignore a link to the actual article
                                if (link.Url.ToString() == article.Url.ToString())
                                    continue;

                                // try and find an author with the link url
                                Author author = FindOrCreateAuthor(link.Url.ToString(), link.Title);
                                newArticle.Author = author.Name;
                            }
                        }

                        newEdition.Articles.Add(newArticle);
                    }

                }
                Editions.Add(newEdition);

            }
        }

        private static bool IsCuratorElementCategory( string categoryName)
        {
            return categoryName == "kymphillpotts" || categoryName == "lucecarter";
        }

        private static string RemoveMarkdownFormatting(string descriptionMarkdown)
        {
            // get one line at a time
            // see if it is in the format [**
            var l = descriptionMarkdown.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (var line in l)
            {
                // is this a by line
                if (line.Contains(" by ") && line.StartsWith("[**") && line.EndsWith(")"))
                    continue;

                if (line.Trim().Length == 0)
                    sb.Append(" ");
                else
                    sb.Append(line);
            }

            return sb.ToString().Trim();
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
            index.Editions = index.Editions.OrderByDescending(o => o.Id).ToList(); 

            string indexJson = JsonConvert.SerializeObject(index,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
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

        private static Author FindOrCreateAuthorByName(string name)
        {
            Author author;

            author = AuthorLookup.Authors.FirstOrDefault(a => a.Name.ToLower() == name.ToLower());
            if (author != null)
            {
                return author;
            }
            else
            {
                author = new Author() { Name = name };
                AuthorLookup.Authors.Add(author);
                return author;
            }
        }

        private static Author FindOrCreateAuthor(string authorKey, string authorDisplayName)
        {
            Author author;

            // see if we can find an auther with the id
            author = AuthorLookup.Authors.FirstOrDefault(a => a.Id.ToLower() == authorKey.ToLower());

            if (author != null)
            {
                return author;
            }

            // now we need to see if we can find an author by alias matching
            foreach (var auth in AuthorLookup.Authors)
            {
                if (auth.Aliases != null)
                {
                    foreach (var alias in auth?.Aliases)
                    {
                        if (alias.Name.ToLower() == authorKey.ToLower())
                        {
                            return auth;
                        }
                    }
                }
            }

            // try and find author by actual name (just for warning output)
            author = AuthorLookup.Authors.FirstOrDefault(a => a.Name.ToLower() == authorDisplayName.ToLower());
            if (author != null)
            {
                return author;
            }

            // at this point we can't find an author so we need to create a new one
            author = new Author();
            author.Id = Guid.NewGuid().ToString();
            author.Name = authorDisplayName;
            author.Aliases = new List<Alias>();
            var newAlias = new Alias();
            newAlias.Name = authorKey.ToLower();
            newAlias.Type = AuthorUrlType(authorKey.ToLower());
            if (newAlias.Type == "twitter")
            {
                author.TwitterHandle = authorKey.Replace("https://twitter.com/", "");
            }
            else if (newAlias.Type == "github")
            {
                author.GitHubHandle = authorKey.Replace("https://github.com/", "");
            }
            else
            {
                author.Aliases.Add(newAlias);
            }

            System.Diagnostics.Debug.WriteLine($"Creating a new author object - Name: {authorDisplayName}, {authorKey}");

            AuthorLookup.Authors.Add(author);

            return author;
        }

        private static string AuthorUrlType(string url)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
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
            return null;

        }

        private static void UpdateIndexFile(Core.Models.Index indexData, List<Edition> curatedEditions)
        {
            // create an index file
            //var indexData = new WeeklyXamarin.Core.Models.Index();
            //indexData.UpdatedTimeStamp = DateTime.UtcNow;

            //indexData.Editions = new List<Core.Models.Edition>();
            foreach (var edition in CuratedEditions)
            {
                Core.Models.Edition ed;
                ed = indexData.Editions.FirstOrDefault(e => e.Id == edition.Number.ToString());
                if (ed == null)
                {
                    ed = edition.ToCoreEdition();
                    indexData.Editions.Add(ed);
                }
                else
                {
                    // update data?
                }
            }
            
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
