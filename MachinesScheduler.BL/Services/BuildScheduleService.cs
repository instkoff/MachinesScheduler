﻿using System.Collections.Generic;
using System.Linq;
using MachinesScheduler.BL.Interfaces;
using MachinesScheduler.BL.Models;

namespace MachinesScheduler.BL.Services
{
    public class BuildScheduleService
    {
        private readonly IPrepareDadaService _dataAccessFromExcel;

        public BuildScheduleService(IPrepareDadaService dataAccessFromExcel)
        {
            _dataAccessFromExcel = dataAccessFromExcel;
        }

        public IEnumerable<Schedule> BuildSchedule()
        {
            var data =_dataAccessFromExcel.PrepearingData();
            var generalSchedule = new List<Schedule>();
            while (data.BatchesQueue.Any())
            {
                var batch = data.BatchesQueue.Dequeue();
                var suitableMachines = data.MachinesList.Where(m => m.TimeDictionary.ContainsKey(batch.NomenclatureId)).ToList();
                var bestMachine = suitableMachines.Aggregate((l, r) =>
                    l.TimeDictionary[batch.NomenclatureId] < r.TimeDictionary[batch.NomenclatureId] ? l : r);
                if (bestMachine.CurrentLoad == 0)
                {
                    var scheduleItem = new Schedule(batch, bestMachine, bestMachine.CurrentLoad);
                    bestMachine.WorksList.Add(batch);
                    generalSchedule.Add(scheduleItem);
                    bestMachine.CurrentLoad += bestMachine.TimeDictionary[batch.NomenclatureId];
                }
                else
                {
                    var minLoadMachine = suitableMachines.Min();
                    var scheduleItem = new Schedule(batch, minLoadMachine, minLoadMachine.CurrentLoad);
                    minLoadMachine.CurrentLoad += minLoadMachine.TimeDictionary[batch.NomenclatureId];
                    minLoadMachine.WorksList.Add(batch);
                    generalSchedule.Add(scheduleItem);
                }
            }

            return generalSchedule;
        }



    }
}