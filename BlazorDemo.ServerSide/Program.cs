using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlazorDemo.Configuration;
using BlazorDemo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Syncfusion.Blazor;
using DevExpress.Blazor.Reporting;
using DevExpress.XtraReports.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.ResponseCompression;
//using StoreProject.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
namespace BlazorDemo.ServerSide
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {

            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(x => Configure(x, args));
        }

        private static void Configure(IWebHostBuilder webHostBuilder, string[] args)
        {
            webHostBuilder
                .UseConfiguration(
                    new ConfigurationBuilder()
                        .AddCommandLine(args)
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("ConnectionStrings.json", false, false)
                        .Build()
                )
                .ConfigureServices(ConfigureServices)
                  .Configure(app =>
                  {
                      app.UseStaticFiles(); // ✅ Enable serving files from wwwroot
                      app.UseRouting();
                      app.UseSession();
                      app.UseRequestLocalization(new RequestLocalizationOptions
                      {
                          DefaultRequestCulture = new RequestCulture("en-IN"),
                          SupportedCultures = new[] { new CultureInfo("en-IN") },
                          SupportedUICultures = new[] { new CultureInfo("en-IN") }
                      });
                      app.UseEndpoints(endpoints =>
                      {

                          endpoints.MapHub<DataHub>("/datahub");
                          endpoints.MapBlazorHub();
                          endpoints.MapFallbackToPage("/_Host");
                      });
                  })
                .UseStaticWebAssets();


            static void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
            {
                var _ = typeof(DynamicExpressionParser).Assembly;
                var configuration = context.Configuration;
                var connectionString = configuration.GetConnectionString("SqlDbContext");
                services.AddSyncfusionBlazor();
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDEzMzI0NEAzMjM5MmUzMDJlMzAzYjMyMzkzYmw3bzlzcXVVT2pUeEQybGxlSmh6b01DVkhZaWhYY2w4YmlwbkNEQVBZU289");
                services.AddSignalR();
                services.AddSingleton(new SqlConnectionConfiguration(connectionString));
                services.AddOptions();
                services.AddHttpContextAccessor();

                services.AddScoped<SessionStateService>();
                //            services.AddDbContext<AppDbContext>(options =>
                //options.UseSqlServer(configuration.GetConnectionString("SqlDbContext")));

                //services.AddSingleton<DataHub>();
                //services.AddScoped<DataChangeNotificationService>();
                //services.AddSingleton<GlobalDataNotifier>();
                //services.AddResponseCompression(opts =>
                //{
                //    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                //        new[] { "application/octet-stream" });
                //});

                services.AddSession(options =>
                {
                    options.IdleTimeout = TimeSpan.FromMinutes(30);
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                });
                services.AddScoped<EmployeeAdoNetService>();
                services.AddScoped<TestService>();
                services.AddScoped<UserSessionService>();
                services.AddSingleton<DemoConfiguration>();
                services.AddScoped<DemoThemeService>();
                services.AddScoped<ReportService>();
                services.AddScoped<MenuService>();
                services.AddScoped<AppStateService>();
              //  services.AddScoped<CookieService>();

                services.AddControllers();
                services.AddHttpClient();
                services.AddScoped<SessionTimeoutService>();
                services.AddHttpContextAccessor();
                services.AddScoped<SessionService>();
                services.AddDevExpressServerSideBlazorReportViewer();
                //  services.AddScoped<AddHttpContextAccessor>();
                services.AddScoped<IDemoStaticResourceService, DemoStaticResourceService>();
                services.AddRazorComponents()

    .AddInteractiveServerComponents();
            }
        }

    }
}
