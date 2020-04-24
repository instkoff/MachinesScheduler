using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ExcelDataReader;
using MachinesScheduler.BL.Models;

namespace MachinesScheduler.BL
{
    public class Data
    {
        public List<Machine> MachinesList { get; } = new List<Machine>();
        public List<Nomenclature> NomenclaturesList { get; } = new List<Nomenclature>();
        public Queue<Batch> BatchesQueue { get; } = new Queue<Batch>();
        public List<Time> TimesList { get; } = new List<Time>();

        public Data()
        {
            LoadNomenclatures();
            LoadBatches();
            LoadTimes();
            LoadMachines();
        }


        private void LoadNomenclatures()
        {
            const string path = @"nomenclatures.xlsx";
            var dataSet = LoadDataSetFromExcel(path);
            var rowsCount = dataSet.Tables[0].Rows.Count;
            var rows = dataSet.Tables[0].Rows;
            for (var i = 1; i < rowsCount; i++)
            {
                if (int.TryParse(rows[i].ItemArray[0].ToString(), out var id))
                {
                    var nomenclature = new Nomenclature(id, rows[i].ItemArray[1].ToString());
                    NomenclaturesList.Add(nomenclature);
                }
                else
                {
                    Console.WriteLine($"Не удалось прочитать номенклатуру под номером {i}");
                }
            }
        }

        private void LoadBatches()
        {
            const string path = @"parties.xlsx";
            var dataSet = LoadDataSetFromExcel(path);
            var rowsCount = dataSet.Tables[0].Rows.Count;
            var rows = dataSet.Tables[0].Rows;
            for (var i = 1; i < rowsCount; i++)
            {
                var batchIdCell = rows[i].ItemArray[0].ToString();
                var nomenclatureIdCell = rows[i].ItemArray[1].ToString();

                if (int.TryParse(batchIdCell, out var id) && int.TryParse(nomenclatureIdCell, out var nomenclatureId))
                {
                    var nomenclature = NomenclaturesList.FirstOrDefault(n => n.Id == nomenclatureId);
                    var batch = new Batch(id, nomenclature);
                    BatchesQueue.Enqueue(batch);

                }
                else
                {
                    Console.WriteLine($"Не удалось прочитать партию под номером {i}");
                }
            }
        }

        private void LoadTimes()
        {
            const string path = @"times.xlsx";
            var dataSet = LoadDataSetFromExcel(path);
            var rowsCount = dataSet.Tables[0].Rows.Count;
            var rows = dataSet.Tables[0].Rows;
            for (var i = 1; i < rowsCount; i++)
            {
                var machineIdCell = rows[i].ItemArray[0].ToString();
                var nomenclatureIdCell = rows[i].ItemArray[1].ToString();
                var operationTimeCell = rows[i].ItemArray[2].ToString();

                if (int.TryParse(machineIdCell, out var machineId) &&
                    int.TryParse(nomenclatureIdCell, out var nomenclatureId) &&
                    int.TryParse(operationTimeCell, out var operationTime))
                {
                    var nomenclature = NomenclaturesList.FirstOrDefault(n => n.Id == nomenclatureId);
                    var time = new Time(nomenclature, operationTime, machineId);
                    TimesList.Add(time);

                }
                else
                {
                    Console.WriteLine($"Не удалось прочитать время обработки под номером {i}");
                }
            }
        }

        private void LoadMachines()
        {
            const string path = @"machine_tools.xlsx";
            var dataSet = LoadDataSetFromExcel(path);
            var rowsCount = dataSet.Tables[0].Rows.Count;
            var rows = dataSet.Tables[0].Rows;
            for (var i = 1; i < rowsCount; i++)
            {
                if (int.TryParse(rows[i].ItemArray[0].ToString(), out var id))
                {
                    var machine = new Machine(id, rows[i].ItemArray[1].ToString());
                    var time = TimesList.Where(t => t.MachineId == id).ToList();
                    machine.TimeList = time;
                    MachinesList.Add(machine);
                }
                else
                {
                    Console.WriteLine($"Не удалось прочитать оборудование под номером {i}");
                }
            }
        }

        private DataSet LoadDataSetFromExcel(string path)
        {
            try
            {
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        return reader.AsDataSet();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

    }
}
