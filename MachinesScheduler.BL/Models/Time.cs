using System;
using Newtonsoft.Json;

namespace MachinesScheduler.BL.Models
{
    /// <summary>
    /// Класс для времени выполнения операции
    /// </summary>
    public class Time : IComparable
    {
        //Id оборудования
        [JsonProperty("Column0")]
        public int MachineId { get; set; }
        //Id номенклатуры
        [JsonProperty("Column1")]
        public int Nomenclatureid { get; set; }
        //Время за которое оборудование обрабатывае номенклатуру
        [JsonProperty("Column2")]
        public int OperationTime { get; set; }
        //Ссылка на номенклатуру
        public Nomenclature Nomenclature { get; set; }

        public override string ToString()
        {
            return Nomenclature.Name + " - " + OperationTime;
        }
        /// <summary>
        /// Сравниваем время по времени обработки
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
