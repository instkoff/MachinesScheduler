using System;

namespace MachinesScheduler.BL.Models
{
    public class Time
    {
        public Nomenclature Nomenclature { get; }
        public int MachineId { get; }
        public int OperationTime { get; }

        public Time()
        {
            
        }

        public Time(Nomenclature nomenclature, int operationTime, int machineId)
        {
            if (nomenclature == null)
                throw new ArgumentNullException("Номенклатура должна быть заполнена.", nameof(nomenclature));
            if (operationTime < 0)
                throw new ArgumentException("Время обработки не может быть меньше нуля.", nameof(operationTime));
            if (machineId < 0)
                throw new ArgumentException("Id машины не может быть меньше нуля.", nameof(operationTime));
            MachineId = machineId;
            Nomenclature = nomenclature;
            OperationTime = operationTime;
        }

        public override string ToString()
        {
            return Nomenclature.Name + " - " + OperationTime;
        }
    }
}
