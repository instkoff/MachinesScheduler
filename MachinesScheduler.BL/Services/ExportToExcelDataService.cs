using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MachinesScheduler.BL.Interfaces;
using MachinesScheduler.BL.Models;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace MachinesScheduler.BL.Services
{
    public class ExportToExcelDataService : IExportDataService
    {
        private readonly ILogger<ExportToExcelDataService> _logger;
        public ExportToExcelDataService(ILogger<ExportToExcelDataService> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Метод для экспорта в эксель готового расписания.
        /// </summary>
        /// <param name="schedule">Список элементов расписания</param>
        /// <returns>Имя готового файла</returns>
        public string Export(IEnumerable<Schedule> schedule)
        {
            //Установка лицензии для EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                var directory = new DirectoryInfo("Расписания");
                if (!directory.Exists)
                    directory.Create();
                //Пытаемся получить информацию о файле, если такой файл уже существует удаляем.
                var file = new FileInfo($"Расписания\\График обработки_{DateTime.Now:dd.MM.yyyy}.xlsx");
                if (file.Exists)
                {
                    file.Delete();
                }
                //Создаем пакет данных эксель
                using (var package = new ExcelPackage(file))
                {
                    //Создаём лист и записываем туда расписание.
                    var mainWorksheet = CreateExcelWorksheet(package, "Общее расписание");
                    var generalSchedule = schedule.ToList();
                    mainWorksheet.Cells[2, 1].LoadFromCollection(generalSchedule, false);

                    //Получаем все машины из расписания и заполняем листы для каждой.
                    var machines = generalSchedule.Select(m => m.Machine).Distinct().OrderBy(m => m.Name);
                    foreach (var machine in machines)
                    {
                        var machineWorkSheet = CreateExcelWorksheet(package, machine.Name);
                        machineWorkSheet.Cells[2, 1].LoadFromCollection(generalSchedule.Where(m => m.Machine == machine));
                    }
                    //Сохраняем в файл и показываем на расположение на экране.
                    package.SaveAs(file);
                    System.Diagnostics.Process.Start("explorer.exe", "/select," + file);
                    return file.FullName;
                }
            }
            //Обрабатываем возможное исключение доступа к файлу.
            catch (IOException e)
            {
                _logger.LogError(e.Message, $"Не могу получить доступ к файлу График обработки.xlsx. Возможно файл открыт в другой программе.");
                throw;
            }
        }
        /// <summary>
        /// Вспомогательный метод для создания листо в книге эксель
        /// </summary>
        /// <param name="package"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private ExcelWorksheet CreateExcelWorksheet(ExcelPackage package, string sheetName)
        {
            var workSheet = package.Workbook.Worksheets.Add(sheetName);
            workSheet.Cells["A1"].Value = "Партия";
            workSheet.Cells["B1"].Value = "Оборудование";
            workSheet.Cells["C1"].Value = "Время начала обработки";
            workSheet.Cells["D1"].Value = "Время конца обработки";
            workSheet.Column(1).Width = 15;
            workSheet.Column(2).Width = 15;
            workSheet.Column(3).Width = 24;
            workSheet.Column(4).Width = 24;
            return workSheet;
        }


    }
}
