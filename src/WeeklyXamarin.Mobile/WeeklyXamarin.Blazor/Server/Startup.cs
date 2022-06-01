using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MonkeyCache;
using MonkeyCache.FileStore;
using System;
using System.Linq;
using WeeklyXamarin.AdminServices.Entities;
using WeeklyXamarin.AdminServices.Services;
using WeeklyXamarin.Blazor.Client.Services;
using WeeklyXamarin.Blazor.Server.Services;
using WeeklyXamarin.Core.Helpers;
using WeeklyXamarin.Core.Services;
using Xamarin.Essentials.Interfaces;

namespace WeeklyXamarin.Blazor.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            var tableConnectString = Configuration.GetValue<string>("containerConnectionString");
            services.AddSingleton(_ => new TableClient<ArticleEntity>(tableConnectString));
            services.AddTransient<ITableService<ArticleEntity>, TableService<ArticleEntity>>();
            services.AddTransient<IArticleStorage, ArticleStorage>();

            services.AddSingleton(_ => new TableClient<AuthorEntity>(tableConnectString));
            services.AddTransient<ITableService<AuthorEntity>, TableService<AuthorEntity>>();
            services.AddTransient<IAuthorStorage, AuthorStorage>();

            services.AddSingleton(_ => new TableClient<EditionEntity>(tableConnectString));
            services.AddTransient<ITableService<EditionEntity>, TableService<EditionEntity>>();
            services.AddTransient<IEditionStorage, EditionStorage>();

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddTransient<IUrlService, UrlService>();
            services.AddTransient<IDataStore, GithubDataStore>();
            services.AddTransient<ICuratedService, CuratedService>();

            services.AddSingleton(_ => Barrel.Current);
            services.AddSingleton<IConnectivity, Connectivity>();
            services.AddSingleton<IAnalytics, WasmAnalytics>();
            services.AddLogging(x => x.AddConsole());
            //services.AddHttpClient();

            services.AddHttpClient(Constants.HttpClientKeys.GitHub, client =>
            {
                client.BaseAddress = new Uri(@"https://raw.githubusercontent.com/weeklyxamarin/WeeklyXamarin.content/master/content/");
            });

            services.AddHttpClient(Constants.HttpClientKeys.Curated, client =>
            {
                client.BaseAddress = new Uri(@"https://api.curated.co/api/v3/");
            });





        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            Barrel.ApplicationId = "WeeklyXamarin";

            app.UseRouting();
        
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
