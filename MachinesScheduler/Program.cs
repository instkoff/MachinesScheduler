using System;
using System.IO;
using System.Text;
using MachinesScheduler.BL.Interfaces;
using MachinesScheduler.BL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MachinesScheduler
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        static void Main(string[] args)
        {
            //Необходимо для работы с ExcelDataReader
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            RegisterServices();
            var scope = _serviceProvider.CreateScope();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            scope.ServiceProvider.GetRequiredService<ConsoleApplication>().Run(config);
            DisposeServices();
            Console.ReadLine();
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ConsoleApplication>();
            services.AddSingleton<ILoadDataService, ExcelDataService>();
            _serviceProvider = services.BuildServiceProvider(true);
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
