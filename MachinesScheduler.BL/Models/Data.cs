using System.Collections.Generic;

namespace MachinesScheduler.BL.Models
{
    /// <summary>
    /// Класс обёртка необходимых данных для построения плана.
    /// </summary>
    public class Data
    {
        //Список оборудования
        public List<Machine> MachinesList { get; }
        //Очередь партий сырья
        public Queue<Batch> BatchesQueue { get; }

        public Data(List<Machine> machines, Queue<Batch> batches)
        {
            MachinesList = machines;
            BatchesQueue = batches;
        }
    }
}
