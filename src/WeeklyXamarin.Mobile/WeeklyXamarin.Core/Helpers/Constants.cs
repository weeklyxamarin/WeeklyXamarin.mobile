using System;
using System.Collections.Generic;
using System.Text;

namespace WeeklyXamarin.Core.Helpers
{
    public static class Constants
    {
        public static class Analytics
        {
            public static class Events
            {
                public const string PageOpened = "Page Opened";
            }

            public static class Properties
            {
                public const string PageName = "Page Name";
                public const string FileName = "Edition File";
                public const string EditionId = "Edition Id";
                public const string ShowSaved = "Show Saved";
            }
        }

        public static class BarrelNames
        {
            public const string SavedArticles = "SavedArticleThing";
        }

        public static class Preferences
        {
            public const string OpenLinksInApp = "OpenLinksInApp";
            public const string Analytics = "Analytics";
        }

        public static class Navigation
        {
            public static class Paths
            {
                public const string Articles = "articles";
                public const string ArticleDetail = "articles/article-detail";
                public const string Editions = "///editions";

            }

            public static class ParameterNames
            {
                public const string ArticleId = "ArticleId";
                public const string EditionId = "EditionId";
            }
        }
    }
}
