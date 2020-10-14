using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WeeklyXamarin.Mobile.Views;
using WeeklyXamarin.Core.ViewModels;
using WeeklyXamarin.Core.Models;
using Container = WeeklyXamarin.Core.Services.Container;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;
using Lottie.Forms;

namespace WeeklyXamarin.Mobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    [QueryProperty(nameof(EditionId), nameof(EditionId))]
    public partial class ArticlesListPage : PageBase<ArticlesListViewModel>
    {
        private readonly bool showSaved;

        public string EditionId { get; set; }
        public ArticlesListPage()
        {
            InitializeComponent();
        }
        public ArticlesListPage(bool showSaved) : this()
        {
            this.showSaved = showSaved;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.ShowSaved = showSaved;
            ViewModel.EditionId = EditionId;

            await ViewModel.LoadArticlesCommand.ExecuteAsync(false);

            //// read the json
            //var assembly = Assembly.GetExecutingAssembly();
            //var resourceName = "WeeklyXamarin.Mobile.bookmarkanimation.json";

            //string json;

            //using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            //using (StreamReader reader = new StreamReader(stream))
            //{
            //    json = reader.ReadToEnd();
            //}

            ////animationView.SetAnimationFromJson(json);

            //animationView.AnimationSource = AnimationSource.Json;
            //animationView.Animation = json;
            //animationView.PlayAnimation();

        }

        //public void SetAnimationFromEmbeddedResource(string resourceName, Assembly assembly = null)
        //{
        //    var animationSource = AnimationSource.EmbeddedResource;
        //    if (assembly == null) 
        //        assembly = Xamarin.Forms.Application.Current.GetType().Assembly; 
        //    Animation = $"resource://{resourceName}?assembly={Uri.EscapeUriString(assembly.FullName)}";
        //}

        private void animationView_OnFailure(object sender, Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e.StackTrace);
        }

        private void animationView_OnAnimationLoaded(object sender, object e)
        {
            System.Diagnostics.Debug.WriteLine("Animation Loaded");

        }

        private void animationView_OnPlayAnimation(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Playing Animation");

        }
    }
}