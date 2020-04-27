using System.Collections.Generic;
using MachinesScheduler.BL.Models;

namespace MachinesScheduler.BL.Interfaces
{
    public interface ILoadDataService
    {
        string Export(IEnumerable<Schedule> schedule);
        IEnumerable<T> Import<T>(string path) where T: class;
    }
}
