using System;
using System.Linq;
using MachinesScheduler.BL.Interfaces;
using MachinesScheduler.BL.Services;
using Microsoft.Extensions.Configuration;

namespace MachinesScheduler
{
    internal class ConsoleApplication
    {
        private readonly ILoadDataService _dataService;

        public ConsoleApplication(ILoadDataService dataService)
        {
            _dataService = dataService;
        }

        public void Run(IConfiguration config)
        {
            var scheduleService = new BuildScheduleService(new PreparedExcelData(_dataService, config));
            var schedule = scheduleService.BuildSchedule();
            var fileName =_dataService.Export(schedule);
            Console.WriteLine(string.Join("\n", fileName));
        }
    }
}