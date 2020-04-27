using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExcelDataReader;
using MachinesScheduler.BL.Interfaces;
using MachinesScheduler.BL.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using Serilog;
using Formatting = Newtonsoft.Json.Formatting;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace MachinesScheduler.BL.Services
{
    public class ExcelDataService : ILoadDataService
    {
        public IEnumerable<T> Import<T>(string path) where T : class
        {
            try
            {
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var dataTable = reader.AsDataSet().Tables[0];
                        dataTable.Rows.RemoveAt(0);
                        var json = JsonConvert.SerializeObject(dataTable, Formatting.Indented);
                        var settings = new JsonSerializerSettings
                        {
                            Converters = new List<JsonConverter> {new CustomIntConverter()}
                        };
                        return JsonConvert.DeserializeObject<IEnumerable<T>>(json, settings);
                    }
                }

            }
            //ToDO Изменить exception
            catch (IOException e)
            {
                Log.Error(e.Message);
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public string Export(IEnumerable<Schedule> schedule)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                var file = new FileInfo(@"График обработки.xlsx");
                if (file.Exists)
                {
                    file.Delete();
                }
                using (var package = new ExcelPackage(file))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Общий план");
                    workSheet.Cells["A1"].Value = "Партия";
                    workSheet.Cells["B1"].Value = "Оборудование";
                    workSheet.Cells["C1"].Value = "Время начала обработки";
                    workSheet.Cells["D1"].Value = "Время конца обработки";
                    workSheet.Column(1).Width = 15;
                    workSheet.Column(2).Width = 15;
                    workSheet.Column(3).Width = 24;
                    workSheet.Column(4).Width = 24;
                    var generalSchedule = schedule.ToList();
                    workSheet.Cells[2, 1].LoadFromCollection(generalSchedule, false);

                    var machines = generalSchedule.Select(m => m.Machine).Distinct().OrderBy(m => m.Name);

                    foreach (var machine in machines)
                    {
                        var machineWorkSheet = package.Workbook.Worksheets.Add($"{machine.Name}");
                        machineWorkSheet.Cells["A1"].Value = "Партия";
                        machineWorkSheet.Cells["B1"].Value = "Оборудование";
                        machineWorkSheet.Cells["C1"].Value = "Время начала обработки";
                        machineWorkSheet.Cells["D1"].Value = "Время конца обработки";
                        machineWorkSheet.Column(1).Width = 15;
                        machineWorkSheet.Column(2).Width = 15;
                        machineWorkSheet.Column(3).Width = 24;
                        machineWorkSheet.Column(4).Width = 24;
                        machineWorkSheet.Cells[2, 1].LoadFromCollection(generalSchedule.Where(m => m.Machine == machine));
                    }
                    package.SaveAs(file);
                    System.Diagnostics.Process.Start("explorer.exe", "/select," + file);
                    return file.FullName;
                }
            }
            catch (IOException e)
            {
                Log.Error(e.Message);
                //Console.WriteLine($"Не могу получить доступ к файлу \n {e.Message}");
                throw;
            }
        }

        private class CustomIntConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(int));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JValue jsonValue = serializer.Deserialize<JValue>(reader);

                if (jsonValue != null && jsonValue.Type == JTokenType.Float)
                {
                    return (int)Math.Round(jsonValue.Value<double>());
                }
                else if (jsonValue != null && jsonValue.Type == JTokenType.Integer)
                {
                    return jsonValue.Value<int>();
                }

                throw new FormatException();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }

}
