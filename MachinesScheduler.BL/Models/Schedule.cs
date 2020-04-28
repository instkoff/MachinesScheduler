namespace MachinesScheduler.BL.Models
{
    /// <summary>
    /// Класс общего элемента расписания
    /// </summary>
    public class Schedule
    {
        //Партия
        public Batch Batch { get; }
        //Оборудование
        public Machine Machine { get; }
        //Время начала операции
        public int StartTime { get; }
        //Время конца операции
        public int EndTime { get; }

        /// <summary>
        /// Конструктор элемента расписания
        /// </summary>
        /// <param name="batch">Партия материалов</param>
        /// <param name="machine">Оборудование для обработки партии</param>
        /// <param name="startTime">Время начала обработки</param>
        public Schedule(Batch batch, Machine machine, int startTime)
        {
            Batch = batch;
            Machine = machine;
            StartTime = startTime;
            EndTime = startTime + machine.TimeDictionary[batch.NomenclatureId];
        }

        public override string ToString()
        {
            return Machine.Name + " - " + Batch.Nomenclature.Name + " " + StartTime + " " + EndTime;
        }
    }

}
