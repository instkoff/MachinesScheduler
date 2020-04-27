using System;
using MachinesScheduler.BL;
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
            var scheduleService = new BuildScheduleService(new Data(_dataService, config));
            var schedule = scheduleService.BuildSchedule();
            Console.WriteLine(string.Join("\n", schedule));
        }
    }
}