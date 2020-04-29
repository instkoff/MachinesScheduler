using System.Collections.Generic;

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
