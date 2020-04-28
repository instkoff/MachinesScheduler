using System;
using System.IO;
using System.Text;
using MachinesScheduler.BL.Interfaces;
using MachinesScheduler.BL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace MachinesScheduler
{
    class Program
    {
        private static IConfigurationRoot Configuration { get; set; }

        static void Main(string[] args)
        {
            //Необходимо для работы с ExcelDataReader
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            try
            {
                Log.Information("Programm start...");
                CreateHostBuilder(args).RunConsoleAsync();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Необработанное исключение");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                    {
                        services
                            .AddSingleton<IConfiguration>(Configuration)
                            .AddHostedService<ConsoleApplication>()
                            .AddTransient<IImportDataService, ImportFromExcelDataService>()
                            .AddTransient<IExportDataService, ExportToExcelDataService>();

                    }
                )
                .ConfigureLogging((hostContext, logging) =>
                    {
                        logging.AddSerilog();
                    });
    }
}
