using System;
using Newtonsoft.Json;

namespace MachinesScheduler.BL.Models
{
    public class Time : IComparable
    {
        [JsonProperty("Column0")]
        public int MachineId { get; set; }
        [JsonProperty("Column1")]
        public int Nomenclatureid { get; set; }
        [JsonProperty("Column2")]
        public int OperationTime { get; set; }
        public Nomenclature Nomenclature { get; set; }

        public override string ToString()
        {
            return Nomenclature.Name + " - " + OperationTime;
        }

        public int CompareTo(object obj)
        {
            if (obj is Time t)
            {
                return OperationTime.CompareTo(t.OperationTime);
            }
            throw new Exception("Невозможно сравнить это оборудование");
        }
    }
}
