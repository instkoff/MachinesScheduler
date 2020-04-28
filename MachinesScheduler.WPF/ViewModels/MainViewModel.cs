using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GalaSoft.MvvmLight;
using MachinesScheduler.BL.Interfaces;
using MachinesScheduler.BL.Models;
using MachinesScheduler.BL.Services;
using Microsoft.Extensions.Options;

namespace MachinesScheduler.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<RectItem> RectItems { get; set; } = new ObservableCollection<RectItem>();
        public ObservableCollection<Schedule> Schedules { get; set; }

        public MainViewModel(IOptions<FilesSettings> filesSettings, IImportDataService importDataService, IExportDataService exportDataService)
        {
            var scheduleService = new BuildScheduleService(new PreparedExcelData(importDataService, filesSettings.Value));
            var schedule = scheduleService.BuildSchedule();
            Schedules = new ObservableCollection<Schedule>(schedule);
            Draw();
            //var fileName = exportDataService.Export(schedule);

        }
        private void Draw()
        {
            var batches = Schedules.Select(b => b.Batch).Where(b=>b.NomenclatureId == 0).ToList();
            Grid grid1 = new Grid();
            int x = 10;
            foreach (var batch in batches)
            {
                RectItems.Add(new RectItem(x,30,50,50));
                x += 60;

            }


        }

    }
}
