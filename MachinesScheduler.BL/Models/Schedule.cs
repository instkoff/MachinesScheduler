namespace MachinesScheduler.BL.Models
{
    public class Schedule
    {
        public Batch Batch { get; }
        public Machine Machine { get; }
        public int StartTime { get; }

        public Schedule(Batch batch, Machine machine, int startTime)
        {
            Batch = batch;
            Machine = machine;
            StartTime = startTime;
        }

        public override string ToString()
        {
            return Machine.Name + " - " + Batch.Nomenclature.Name + " " + StartTime;
        }
    }

}
