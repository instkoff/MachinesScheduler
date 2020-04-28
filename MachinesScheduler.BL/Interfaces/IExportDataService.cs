using System.Collections.Generic;
using MachinesScheduler.BL.Models;

namespace MachinesScheduler.BL.Interfaces
{
    /// <summary>
    /// Интерфейс для экспорта данных куда либо.
    /// </summary>
    public interface IExportDataService
    {
        string Export(IEnumerable<Schedule> schedule);
    }
}
