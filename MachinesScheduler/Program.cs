using System;
using System.Text;
using MachinesScheduler.BL.Interfaces;
using MachinesScheduler.BL.Models;
using MachinesScheduler.BL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace MachinesScheduler
{
    class Program
    { 
        static void Main(string[] args)
        {
            //Необходимо для работы с ExcelDataReader
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //Пытаемся стартануть узел
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

        //Создаём универсальный узел(он же хост)
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            new HostBuilder()
                //получаем файл конфигурации
                .ConfigureAppConfiguration((hostContext, builder) =>
                {
                    builder.AddJsonFile("appsettings.json", optional: true);
                })
                //настраиваем сервисы
                .ConfigureServices((hostContext, services) =>
                    {
                        services
                            .Configure<FilesSettings>(hostContext.Configuration.GetSection(nameof(FilesSettings)))
                            .AddHostedService<ConsoleApplication>()
                            .AddScoped<IImportDataService, ImportFromExcelDataService>()
                            .AddScoped<IExportDataService, ExportToExcelDataService>();

                    }
                )
                //Добавляем логгирование, в данном случае Serilog
                .ConfigureLogging((hostContext, logging) =>
                    {
                        Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(hostContext.Configuration)
                            .CreateLogger();

                        logging.AddSerilog();
                    });
    }
}
