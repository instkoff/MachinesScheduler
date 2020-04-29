using System;
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
            //Создаём узел с настройками
            _host = Host.CreateDefaultBuilder()  
                //Получаем файл конфигурации
                    .ConfigureAppConfiguration((context, builder) =>
                    {
                        builder.AddJsonFile("appsettings.json", optional: true);

                    })
                //Настраиваем зависимости
                    .ConfigureServices((context, services) =>
                    {
                        ConfigureServices(context.Configuration, services);
                    })
                //Настраиваем логгер
                    .ConfigureLogging((context, logging) =>
                    {
                        Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(context.Configuration)
                            .CreateLogger();
                        logging.AddSerilog();
                    })
                    .Build();
            ServiceProvider = _host.Services;
        }

        private void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
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
