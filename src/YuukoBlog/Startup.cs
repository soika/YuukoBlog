namespace YuukoBlog
{
    using System;
    using System.IO;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Models;
    using Pomelo.AspNetCore.Localization;
    using WebMarkupMin.AspNetCore1;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            IConfiguration Configuration;
            services.AddConfiguration(out Configuration);

            if (Configuration["Database:Type"] == "SQLite")
            {
                services.AddDbContext<BlogContext>(x => x.UseSqlite(Configuration["Database:ConnectionString"]));
            }
            else if (Configuration["Database:Type"] == "MySQL")
            {
                services.AddDbContext<BlogContext>(x => x.UseMySql(Configuration["Database:ConnectionString"]));
            }

            services.AddSmartCookies();

            services.AddMemoryCache();
            services.AddSession(x => x.IdleTimeout = TimeSpan.FromMinutes(20));

            services.AddBlobStorage()
                .AddEntityFrameworkStorage<BlogContext>()
                .AddSessionUploadAuthorization();

            services.AddPomeloLocalization(x =>
            {
                x.AddCulture(new[] {"zh", "zh-CN", "zh-Hans", "zh-Hans-CN", "zh-cn"},
                    new JsonLocalizedStringStore(Path.Combine("Localization", "zh-CN.json")));
                x.AddCulture(new[] {"en", "en-US", "en-GB"},
                    new JsonLocalizedStringStore(Path.Combine("Localization", "en-US.json")));
                x.AddCulture(new[] { "vi", "vi-VN", "vi-VN" },
                    new JsonLocalizedStringStore(Path.Combine("Localization", "vi-VN.json")),
                    true);
            });

            services.AddMvc()
                .AddMultiTemplateEngine()
                .AddCookieTemplateProvider();

            //services.AddWebMarkupMin(
            //        options =>
            //        {
            //            options.AllowMinificationInDevelopmentEnvironment = true;
            //            options.AllowCompressionInDevelopmentEnvironment = true;
            //        })
            //    .AddHtmlMinification(
            //        options =>
            //        {
            //            options.MinificationSettings.RemoveRedundantAttributes = true;
            //            options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
            //            options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
            //        })
            //    .AddHttpCompression();

            //services.AddTimedJob();
        }

        public async void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Warning, true);

            app.UseStaticFiles();
            //app.UseWebMarkupMin();
            app.UseSession();
            app.UseBlobStorage("/assets/shared/scripts/jquery.codecomb.fileupload.js");
            app.UseDeveloperExceptionPage();
            app.UseMvcWithDefaultRoute();

            await SampleData.InitializeYuukoBlog(app.ApplicationServices);

            //app.UseTimedJob();
        }

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseIISIntegration()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}