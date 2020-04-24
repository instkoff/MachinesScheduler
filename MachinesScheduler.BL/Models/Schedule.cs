namespace MachinesScheduler.BL.Models
{
    public class Schedule
    {
        public Batch Batch { get; set; }
        public Machine Machine { get; set; }
        public int StartTime { get; set; }
    }
}
