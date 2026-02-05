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
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<BlazorDemo.AbraqAccount.Data.AppDbContext>();
                    await BlazorDemo.AbraqAccount.Data.DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    
                     Console.WriteLine($"AbraqAccount DB Init Error: {ex.Message}");
                }
            }

            await host.RunAsync();
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

                // AbraqAccount Services Registration
                services.AddDbContext<BlazorDemo.AbraqAccount.Data.AppDbContext>(options =>
                    options.UseSqlServer(
                        "Server=localhost\\SQLEXPRESS;Database=AbraqDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true",
                        sqlOptions => sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null
                        )
                    ));

                // Add DbContext Factory for Blazor
                services.AddDbContextFactory<BlazorDemo.AbraqAccount.Data.AppDbContext>(options =>
                    options.UseSqlServer(
                        "Server=localhost\\SQLEXPRESS;Database=AbraqDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
                    ), lifetime: ServiceLifetime.Scoped);

                // Register AbraqAccount Services
               // services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.IAccountService, BlazorDemo.AbraqAccount.Services.Implementations.AccountService>();
               // services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.IDashboardService, BlazorDemo.AbraqAccount.Services.Implementations.DashboardService>();
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.IVendorService, BlazorDemo.AbraqAccount.Services.Implementations.VendorService>();
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.ISettingsService, BlazorDemo.AbraqAccount.Services.Implementations.SettingsService>();
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.IBankMasterService, BlazorDemo.AbraqAccount.Services.Implementations.BankMasterService>();
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.IExpensesIncurredService, BlazorDemo.AbraqAccount.Services.Implementations.ExpensesIncurredService>();
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.IPaymentSettlementService, BlazorDemo.AbraqAccount.Services.Implementations.PaymentSettlementService>();
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.IReceiptEntryService, BlazorDemo.AbraqAccount.Services.Implementations.ReceiptEntryService>();
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.IGeneralEntryService, BlazorDemo.AbraqAccount.Services.Implementations.GeneralEntryService>();
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.IAccountMasterService, BlazorDemo.AbraqAccount.Services.Implementations.AccountMasterService>();
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.ITransactionEntriesService, BlazorDemo.AbraqAccount.Services.Implementations.TransactionEntriesService>();
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.IPurchaseMasterService, BlazorDemo.AbraqAccount.Services.Implementations.PurchaseMasterService>();
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.IPurchaseOrderService, BlazorDemo.AbraqAccount.Services.Implementations.PurchaseOrderService>();
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.IPurchaseTransactionService, BlazorDemo.AbraqAccount.Services.Implementations.PurchaseTransactionService>();
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.IInventoryService, BlazorDemo.AbraqAccount.Services.Implementations.InventoryService>();
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.IAgriMasterService, BlazorDemo.AbraqAccount.Services.Implementations.AgriMasterService>();
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.IPackingService, BlazorDemo.AbraqAccount.Services.Implementations.PackingService>();
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.IDebitNoteService, BlazorDemo.AbraqAccount.Services.Implementations.DebitNoteService>();
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.ICreditNoteService, BlazorDemo.AbraqAccount.Services.Implementations.CreditNoteService>();

                // Resolve ambiguity for ReportService if necessary, assuming BlazorDemo.AbraqAccount.Services.Interfaces.IReportService is distinctive or we use full qualification.
                // Note: There is already a ReportService registered on line 109. If names collide, we must use full names.
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.IReportService, BlazorDemo.AbraqAccount.Services.Implementations.ReportService>();
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.IUOMService, BlazorDemo.AbraqAccount.Services.Implementations.UOMService>();
                services.AddScoped<BlazorDemo.AbraqAccount.Services.Interfaces.INotificationService, BlazorDemo.AbraqAccount.Services.Implementations.NotificationService>();

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
