using Blazored.LocalStorage;
using MatBlazor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MonkeyCache;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeeklyXamarin.Blazor.Client.Services;
using WeeklyXamarin.Core.Services;
using WeeklyXamarin.Core.ViewModels;
using Xamarin.Essentials.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using WeeklyXamarin.Core.Helpers;

namespace WeeklyXamarin.Blazor.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddHttpClient(Constants.HttpClientKeys.GitHub, client =>
            {
                client.BaseAddress = new Uri(@"https://raw.githubusercontent.com/weeklyxamarin/WeeklyXamarin.content/master/content/");
            });

            builder.Services.AddHttpClient(Constants.HttpClientKeys.WeeklyXamarin, client =>
            {
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            });

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            //builder.Services.AddTransient<HttpClient>(_ => new HttpClient());
            builder.Services.AddScoped<IDataStore, GithubDataStore>();
            builder.Services.AddScoped<IArticleRestService, ArticleRestService> ();
            builder.Services.AddScoped<ICuratedRestService, CuratedRestService> ();
            builder.Services.AddSingleton<IConnectivity, WasmConnectivity>();
            builder.Services.AddSingleton<IAnalytics, WasmAnalytics>();
            builder.Services.AddScoped<IBarrel, WasmBarrel>();
            builder.Services.AddSingleton<INavigationService, WasmNavigationService>();
            builder.Services.AddSingleton<IMessagingService, WasmMessagingService>();
            builder.Services.AddSingleton<IBrowser, WasmBrowser>();
            builder.Services.AddSingleton<IPreferences, WasmPreferences>();
            builder.Services.AddSingleton<IThemeService, WasmThemeService>();
            builder.Services.AddSingleton<IShare, WasmShare>();
            builder.Services.AddTransient<EditionsViewModel>();
            builder.Services.AddTransient<ArticlesListViewModel>();
            builder.Services.AddTransient<AboutViewModel>();
            builder.Services.AddTransient<AcknowledgementsViewModel>();
            builder.Services.AddTransient<AuthorViewModel>();
            builder.Services.AddTransient<AuthorsViewModel>();
            builder.Services.AddTransient<SearchViewModel>();
            builder.Services.AddSingleton<ILogger, WasmLogger<GithubDataStore>>();
            builder.Services.AddMatBlazor();
            

            await builder.Build().RunAsync();
        }

        public static Version Version { get; set; } = typeof(Program).Assembly
                        .GetName().Version ?? new();
        public static string ProductVersion { get; set; } = typeof(Program).Assembly
                                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                                .InformationalVersion ?? "";

    }
}
