using System.Collections.Generic;

namespace MachinesScheduler.BL.Interfaces
{
    public interface ILoadDataService
    {
        IEnumerable<T> Load<T>(string path) where T: class;
    }
}
