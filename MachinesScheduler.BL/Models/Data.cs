using System.Collections.Generic;

namespace MachinesScheduler.BL.Models
{
    public class Data
    {
        public List<Machine> MachinesList { get; }
        public Queue<Batch> BatchesQueue { get; }

        public Data(List<Machine> machines, Queue<Batch> batches)
        {
            MachinesList = machines;
            BatchesQueue = batches;
        }
    }
}
