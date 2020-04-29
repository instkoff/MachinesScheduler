using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using MachinesScheduler.BL.Interfaces;
using MachinesScheduler.BL.Models;
using MachinesScheduler.BL.Services;
using MachinesScheduler.WPF.Shapes;
using Microsoft.Extensions.Options;

namespace MachinesScheduler.WPF.ViewModels
{
    /// <summary>
    /// ViewModel главного окна
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IOptions<FilesSettings> _filesSettings;
        private readonly IImportDataService _importDataService;
        private readonly IExportDataService _exportDataService;
        public List<RectItem> RectItems { get; } = new List<RectItem>();
        public List<TextDetails> TextItems { get; } = new List<TextDetails>();
        public List<TimeLine> TimeLines { get; } = new List<TimeLine>();
        public List<Schedule> Schedule { get; private set; }
        //Пробрасываем зависимости
        public MainViewModel(IOptions<FilesSettings> filesSettings, IImportDataService importDataService, IExportDataService exportDataService)
        {
            _filesSettings = filesSettings;
            _importDataService = importDataService;
            _exportDataService = exportDataService;
            Initialize();
        }

        //Инициализируем все данные
        private void Initialize()
        {
            try
            {
                //получаем сервис построения расписания
                var scheduleService = new BuildScheduleService(new PreparedExcelData(_importDataService, _filesSettings.Value));
                //Создаём расписание
                Schedule = scheduleService.BuildSchedule().ToList();
                //рисуем расписание
                DrawSchedule();
                //Экспортируем в эксель
                var fileName = _exportDataService.Export(Schedule);
                MessageBox.Show($"Ваше готовое расписание находится в {fileName}");
            }
            //Если что-то пошло не так, ловим.
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Application.Current.Shutdown();
                throw;
            }


        }

        /// <summary>
        /// Метод для отрисовки расписания
        /// </summary>
        private void DrawSchedule()
        {
            var machines = (Schedule ?? throw new ArgumentNullException()).Select(m => m.Machine).Distinct().OrderBy(m=>m.Name).ToList();
            var y = 10;
            var timeLinePoint = 0;
            foreach (var machine in machines)
            {
                TextItems.Add(new TextDetails(10, y-35, 50,50, machine.Name));
                RectItems.Add(new RectItem(10, y, 50, 50, "Black"));
                var batches = Schedule.Where(m=>m.Machine.Id == machine.Id).Select(b=>b.Batch).ToList();
                var x = 65;
                foreach (var b in batches)
                {
                    var color = b.NomenclatureId switch
                    {
                        0 => "Gold",
                        1 => "Silver",
                        2 => "LightSteelBlue",
                        _ => "Black"
                    };
                    timeLinePoint += machine.TimeDictionary[b.NomenclatureId];
                    var widthFromTime = machine.TimeDictionary[b.NomenclatureId]+15;
                    TextItems.Add(new TextDetails(x + widthFromTime, y + 32, 20, 20, timeLinePoint.ToString()));
                    TextItems.Add(new TextDetails(x, y, widthFromTime, 20, b.Nomenclature.Name.Substring(0,3)));
                    RectItems.Add(new RectItem(x,y,widthFromTime,50, color));
                    x += widthFromTime+2;
                }
                TimeLines.Add(new TimeLine(10, y+23, x+10, y+23));
                y += 90;
                timeLinePoint = 0;
            }

        }

    }
}
