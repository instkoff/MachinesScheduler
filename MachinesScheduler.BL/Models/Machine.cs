using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MachinesScheduler.BL.Models
{
    public class Machine : IComparable
    {
        [JsonProperty("Column0")]
        public int Id { get; set; }
        [JsonProperty("Column1")]
        public string Name { get; set; }
        public Dictionary<int,int> TimeDictionary { get; set; }
        public List<Batch> WorksList { get; }
        public int CurrentLoad { get; set; }

        public Machine()
        {
            Id = 0;
            Name = string.Empty;
            TimeDictionary = new Dictionary<int, int>();
            WorksList = new List<Batch>();
            CurrentLoad = 0;
        }

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
