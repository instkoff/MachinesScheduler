using System.Threading;
using System.Threading.Tasks;
using MachinesScheduler.BL.Interfaces;
using MachinesScheduler.BL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace MachinesScheduler
{
    public class ConsoleApplication : IHostedService
    {
        private readonly IImportDataService _importDataService;
        private readonly IExportDataService _exportDataService;
        private readonly IConfiguration _config;

        public ConsoleApplication(IImportDataService importDataService, IExportDataService exportDataService, IConfiguration config)
        {
            _importDataService = importDataService;
            _exportDataService = exportDataService;
            _config = config;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var scheduleService = new BuildScheduleService(new PreparedExcelData(_importDataService, _config.GetSection("FilesSettings")));
            var schedule = scheduleService.BuildSchedule();
            var fileName = _exportDataService.Export(schedule);
            Log.Information($"Файл с расписанием: {fileName}");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}