using MachinesScheduler.BL.Models;

namespace MachinesScheduler.BL.Interfaces
{
    /// <summary>
    /// Интерфейс для подготовки данных
    /// </summary>
    public interface IPrepareDadaService
    {
        Data PrepearingData();
    }
}