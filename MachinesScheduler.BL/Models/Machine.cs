using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MachinesScheduler.BL.Models
{
    /// <summary>
    /// Класс оборудования
    /// </summary>
    public class Machine : IComparable
    {
        //Id
        [JsonProperty("Column0")]
        public int Id { get; set; }
        //Название
        [JsonProperty("Column1")]
        public string Name { get; set; }
        //Словарь по которому удобно находить время обработки по Id номенклатуры
        public Dictionary<int,int> TimeDictionary { get; set; }
        //Список партия для экземпляра оборудования
        public List<Batch> WorksList { get; }
        //Текущая загрузка оборудования
        public int CurrentLoad { get; set; }

        public Machine()
        {
            Id = 0;
            Name = string.Empty;
            TimeDictionary = new Dictionary<int, int>();
            WorksList = new List<Batch>();
            CurrentLoad = 0;
        }

        /// <summary>
        /// Сравнение оборудования между собой по нагрузке
        /// </summary>
        /// <param name="obj">Машина для сравнения</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj is Machine m)
            {
                return CurrentLoad.CompareTo(m.CurrentLoad);
            }
            throw new Exception("Невозможно сравнить это оборудование");
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
