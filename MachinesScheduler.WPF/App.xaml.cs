using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows;
using MachinesScheduler.BL.Interfaces;
using MachinesScheduler.BL.Models;
using MachinesScheduler.BL.Services;
using MachinesScheduler.WPF.ViewModels;
using MachinesScheduler.WPF.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace MachinesScheduler.WPF
{
    public partial class App
    {
        private readonly IHost _host;
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            _host = Host.CreateDefaultBuilder()  
                    .ConfigureAppConfiguration((context, builder) =>
                    {
                        builder.AddJsonFile("appsettings.json", optional: true);

                    }).ConfigureServices((context, services) =>
                    {
                        ConfigureServices(context.Configuration, services);
                    })
                    .ConfigureLogging(logging => { logging.AddSerilog(); })
                    .Build();

            ServiceProvider = _host.Services;
        }

        private void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.Configure<FilesSettings>(configuration.GetSection(nameof(FilesSettings)));
            services.AddScoped<IImportDataService, ImportFromExcelDataService>();
            services.AddScoped<IExportDataService, ExportToExcelDataService>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<MainWindow>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            //Необходимо для работы с ExcelDataReader
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            await _host.StartAsync();

            var window = ServiceProvider.GetRequiredService<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }
    }
}
