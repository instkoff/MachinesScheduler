using System.Collections.Generic;
using MachinesScheduler.BL.Models;

namespace MachinesScheduler.BL.Interfaces
{
    /// <summary>
    /// Интерфейс для загрузки данных
    /// </summary>
    public interface IImportDataService
    {
        IEnumerable<T> Import<T>(string path) where T: class;
    }
}
