using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.Models;
using WeeklyXamarin.Core.Services;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace WeeklyXamarin.Core.ViewModels
{
    public abstract class ArticleViewModelBase : ViewModelBase
    {
        private string _bookmarkIcon;
        protected readonly IShare share;
        protected readonly IDataStore dataStore;
        protected ArticleViewModelBase(INavigationService navigation,
                                       IShare share,
                                       IDataStore dataStore,
                                       IAnalytics analytics,
                                       IMessagingService messagingService) : base(navigation,
                                                                                  analytics,
                                                                                  messagingService)
        {
            this.share = share;
            this.dataStore = dataStore;
            ShareCommand = new AsyncCommand<Article>(ExecuteShareCommand);
            ToggleBookmarkCommand = new Command<Article>(ExecuteToggleBookmarkArticle);
        }

        public string BookmarkIcon
        {
            get => _bookmarkIcon;
            set => SetProperty(ref _bookmarkIcon, value);
        }

        public ICommand ToggleBookmarkCommand { get; set; }

        public ICommand ShareCommand { get; set; }

        protected virtual void ExecuteToggleBookmarkArticle(Article article)
        {
            if (article.IsSaved)
            {
                dataStore.UnbookmarkArticle(article);
                article.IsSaved = false;
                BookmarkIcon = Constants.ToolbarIcons.Bookmark;
            }
            else
            {
                dataStore.BookmarkArticle(article);
                article.IsSaved = true;
                BookmarkIcon = Constants.ToolbarIcons.Unbookmark;
            }

            messagingService.Send(article, "BOOKMARKED");
        }

        private async Task ExecuteShareCommand(Article article)
        {
            await share.RequestAsync(new ShareTextRequest
            {
                Uri = article.Url,
                Title = article.Title
            });
        }
    }
}
