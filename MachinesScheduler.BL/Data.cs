using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ExcelDataReader;
using MachinesScheduler.BL.Models;

namespace MachinesScheduler.BL
{
    public class Data
    {
        public List<Machine> LoadMachines()
        {
            var machines = new List<Machine>();
            const string path = @"machine_tools.xlsx";
            var machinesDataSet = LoadDataSetFromExcel(path);
            var rowsCount = machinesDataSet.Tables[0].Rows.Count;
            var rows = machinesDataSet.Tables[0].Rows;
            for (var i = 1; i < rowsCount; i++)
            {
                if (int.TryParse(rows[i].ItemArray[0].ToString(), out var id))
                {
                    var machine = new Machine(id, rows[i].ItemArray[1].ToString());
                    machines.Add(machine);
                }
                else
                {
                    Console.WriteLine($"Не удалось прочитать оборудование под номером {i}");
                }
            }

            return machines;
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
