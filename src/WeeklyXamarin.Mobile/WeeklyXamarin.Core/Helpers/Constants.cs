using System;
using System.Collections.Generic;
using System.Text;

namespace WeeklyXamarin.Core.Helpers
{
    public static class Constants
    {
        public static class BarrelNames
        {
            public const string SavedArticles = "SavedArticleThing";
        }

        public static class Navigation
        {
            public static class Paths
            {
                public const string Articles = "articles";
                public const string ArticleDetail = "articles/article-detail";

            }

            public static class ParameterNames
            {
                public const string ArticleId = "ArticleId";
                public const string EditionId = "EditionId";
            }
        }
    }
}
