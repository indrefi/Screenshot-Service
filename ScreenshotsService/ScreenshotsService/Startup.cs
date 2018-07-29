using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScreenshotsService.Models;
using ScreenshotsService.Services;
using ScreenshotsService.Services.Interfaces;
using ScreenshotsService.UtilServices;
using ScreenshotsService.UtilServices.Interfaces;

namespace ScreenshotsService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Add custom services
            services.AddTransient<IProcessImage, ProcessImagePhantomJS>();
            services.AddTransient<IPersistData, PersistToS3>();
            services.AddTransient<IHashService, ComputeSHA256>();
            services.AddTransient<ICollectSystemInfo, CollectSystemInfo>();
            services.AddTransient<IOpenPages, OpenPagesWithDefaultBrowser>();
            services.AddTransient<ILoadData, LoadImageFromS3>();
            services.AddTransient<IExecute, ExecuteProcessing>();

            services.AddSingleton<IDisplaySize, DisplaySize>();
            services.AddSingleton<IConnectToS3, ConnectToS3>();

            // Add configuration options
            services.Configure<ImageConfigModel>(Configuration.GetSection("ImageConfig"));
            services.Configure<S3SettingsModel>(Configuration.GetSection("S3Settings"));
            services.Configure<DisplaySizeSettingsModel>(Configuration.GetSection("DisplaySizeSettings"));
            services.Configure<PhantomJsSettingsModel>(Configuration.GetSection("PhantomJsSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

        }
    }
}
