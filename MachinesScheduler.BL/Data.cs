using System.Collections.Generic;
using System.Linq;
using MachinesScheduler.BL.Interfaces;
using MachinesScheduler.BL.Models;
using Microsoft.Extensions.Configuration;

namespace MachinesScheduler.BL
{
    public class Data
    {
        private readonly ILoadDataService _dataService;
        private readonly IConfiguration _configuration;
        public List<Machine> MachinesList { get; private set; }
        public List<Nomenclature> NomenclaturesList { get; private set; }
        public Queue<Batch> BatchesQueue { get; }
        public List<Time> TimesList { get; private set; }

        public Data(ILoadDataService dataService, IConfiguration config)
        {
            _dataService = dataService;
            BatchesQueue = new Queue<Batch>();
            _configuration = config;
        }

        public void PrepearingData()
        {
            NomenclaturesList = _dataService.Load<Nomenclature>(_configuration["Nomenclature"]).ToList();
            MachinesList = _dataService.Load<Machine>(_configuration["Machines"]).ToList();
            TimesList = _dataService.Load<Time>(_configuration["Times"]).ToList();
            var batches = _dataService.Load<Batch>(_configuration["Batches"]).ToList();

            batches.ForEach(b => b.Nomenclature = NomenclaturesList.Find(n => n.Id == b.NomenclatureId));
            batches.ForEach(b => BatchesQueue.Enqueue(b));

            TimesList.ForEach(t => t.Nomenclature = NomenclaturesList.Find(n => n.Id == t.Nomenclatureid));
            foreach (var machine in MachinesList)
            {
                var times = TimesList.Where(tl => tl.MachineId == machine.Id).ToList();
                times.ForEach(t => machine.TimeDictionary.Add(t.Nomenclatureid, t.OperationTime));

            }
        }

    }
}
