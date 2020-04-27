using System.Collections.Generic;

namespace MachinesScheduler.BL.Interfaces
{
    public interface ILoadDataService
    {
        IList<T> Load<T>(string path) where T: class;
    }
}
