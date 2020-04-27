using System.Collections.Generic;
using System.Linq;
using MachinesScheduler.BL.Interfaces;
using MachinesScheduler.BL.Models;
using Microsoft.Extensions.Configuration;

namespace MachinesScheduler.BL.Services
{
    public class PreparedExcelData : IPrepareDadaService
    {
        private readonly ILoadDataService _dataService;
        private readonly IConfiguration _configuration;
        private List<Time> TimesList { get; set; }
        private List<Nomenclature> NomenclaturesList { get; set; }
        private Queue<Batch> BatchesQueue { get; set; }
        private List<Machine> MachinesList { get; set; }

        public PreparedExcelData(ILoadDataService dataService, IConfiguration config)
        {
            _dataService = dataService;
            _configuration = config;
        }

        public Data PrepearingData()
        {
            NomenclaturesList = _dataService.Load<Nomenclature>(_configuration["Nomenclature"]).ToList();
            MachinesList = _dataService.Load<Machine>(_configuration["Machines"]).ToList();
            TimesList = _dataService.Load<Time>(_configuration["Times"]).ToList();
            var batches = _dataService.Load<Batch>(_configuration["Batches"]).ToList();

            batches.ForEach(b => b.Nomenclature = NomenclaturesList.Find(n => n.Id == b.NomenclatureId));
            BatchesQueue = new Queue<Batch>(batches);

            TimesList.ForEach(t => t.Nomenclature = NomenclaturesList.Find(n => n.Id == t.Nomenclatureid));

            foreach (var machine in MachinesList)
            {
                var times = TimesList.Where(tl => tl.MachineId == machine.Id);
                machine.TimeDictionary = times.ToDictionary(t => t.Nomenclatureid, o => o.OperationTime);
            }
            var data = new Data(MachinesList, BatchesQueue);
            return data;
        }

    }
}
