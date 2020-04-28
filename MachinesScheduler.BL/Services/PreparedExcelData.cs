using System.Collections.Generic;
using System.Linq;
using MachinesScheduler.BL.Interfaces;
using MachinesScheduler.BL.Models;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace MachinesScheduler.BL.Services
{
    /// <summary>
    /// Класс для подготовки данных для последующей обработки.
    /// </summary>
    public class PreparedExcelData : IPrepareDadaService
    {
        private readonly IImportDataService _dataService;
        private readonly IConfiguration _configuration;
        private List<Time> TimesList { get; set; }
        private List<Nomenclature> NomenclaturesList { get; set; }
        private Queue<Batch> BatchesQueue { get; set; }
        private List<Machine> MachinesList { get; set; }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dataService">Экземпляр интерфейса для получения необработанных данных откуда либо.</param>
        /// <param name="config">Экземпляр объекта конфигурации для получения названий файлов.</param>
        public PreparedExcelData(IImportDataService dataService, IConfiguration config)
        {
            _dataService = dataService;
            _configuration = config;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Data PrepearingData()
        {
            //Импортируем данные из эксель.
            NomenclaturesList = _dataService.Import<Nomenclature>(_configuration["Nomenclature"]).ToList();
            MachinesList = _dataService.Import<Machine>(_configuration["Machines"]).ToList();
            TimesList = _dataService.Import<Time>(_configuration["Times"]).ToList();
            var batches = _dataService.Import<Batch>(_configuration["Batches"]).ToList();
            //Заполняем очередь партий, проставляем ссылки на нужные объекты при необходимости.
            batches.ForEach(b => b.Nomenclature = NomenclaturesList.Find(n => n.Id == b.NomenclatureId));
            BatchesQueue = new Queue<Batch>(batches);

            TimesList.ForEach(t => t.Nomenclature = NomenclaturesList.Find(n => n.Id == t.Nomenclatureid));

            foreach (var machine in MachinesList)
            {
                var times = TimesList.Where(tl => tl.MachineId == machine.Id);
                machine.TimeDictionary = times.ToDictionary(t => t.Nomenclatureid, o => o.OperationTime);
            }
            var data = new Data(MachinesList, BatchesQueue);
            Log.Information("Данные подготовлены.");
            //Возвращаем подготовленные данные.
            return data;
        }

    }
}
