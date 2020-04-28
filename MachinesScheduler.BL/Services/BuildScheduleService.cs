using System.Collections.Generic;
using System.Linq;
using MachinesScheduler.BL.Interfaces;
using MachinesScheduler.BL.Models;
using Serilog;

namespace MachinesScheduler.BL.Services
{
    /// <summary>
    /// Клас для построения расписания
    /// </summary>
    public class BuildScheduleService
    {

        private readonly IPrepareDadaService _dataAccess;
        private Data _data;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dataAccess">Экземпляр доступа к данным</param>
        public BuildScheduleService(IPrepareDadaService dataAccess)
        {
            _dataAccess = dataAccess;
        }

        /// <summary>
        /// Метод создающий расписание
        /// </summary>
        /// <returns>Возвращает общее расписание</returns>
        public IEnumerable<Schedule> BuildSchedule()
        {
            //Получаем подготовленные данные
            _data =_dataAccess.PrepearingData();
            //Создаём общее пустое расписание
            var generalSchedule = new List<Schedule>();
            //Пока в очереди есть элементы обрабатываем партии
            while (_data.BatchesQueue.Any())
            {
                //Извлекаем партию
                var batch = _data.BatchesQueue.Dequeue();
                //Находим наиболее подходящее оборудование
                var suitableMachines = _data.MachinesList.Where(m => m.TimeDictionary.ContainsKey(batch.NomenclatureId)).ToList();
                //Находим наилучшую печь для данной партии
                var bestMachine = suitableMachines.Aggregate((l, r) =>
                    l.TimeDictionary[batch.NomenclatureId] < r.TimeDictionary[batch.NomenclatureId] ? l : r);
                //если она ещё не загружена, грузим в лучшую
                if (bestMachine.CurrentLoad == 0)
                {
                    var scheduleItem = new Schedule(batch, bestMachine, bestMachine.CurrentLoad);
                    bestMachine.WorksList.Add(batch);
                    generalSchedule.Add(scheduleItem);
                    bestMachine.CurrentLoad += bestMachine.TimeDictionary[batch.NomenclatureId];
                }
                //далее грузим в наименее загруженную из тех которые могу обработать данную партию
                else
                {
                    var minLoadMachine = suitableMachines.Min();
                    var scheduleItem = new Schedule(batch, minLoadMachine, minLoadMachine.CurrentLoad);
                    minLoadMachine.CurrentLoad += minLoadMachine.TimeDictionary[batch.NomenclatureId];
                    minLoadMachine.WorksList.Add(batch);
                    generalSchedule.Add(scheduleItem);
                }
            }
            Log.Information("Расписание построено.");
            return generalSchedule;
        }
    }
}
