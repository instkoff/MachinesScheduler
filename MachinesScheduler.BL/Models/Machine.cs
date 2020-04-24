using System;
using System.Collections.Generic;
using System.Linq;

namespace MachinesScheduler.BL.Models
{
    public class Machine
    {
        public int Id { get; }
        public string Name { get; }
        public List<Time> TimeList { get; set; }
        public List<Batch> WorksList { get; set; }
        private int CurrentLoad { get; set; }

        public Machine()
        {
            Id = 0;
            Name = string.Empty;
            TimeList = new List<Time>();
            WorksList = new List<Batch>();
            CurrentLoad = 0;
        }

        public Machine(int id, string name)
        {
            if (id < 0)
            {
                throw new ArgumentException("Id не может быть меньше нуля.", nameof(id));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Имя оборудования должно быть заполнено", nameof(name));
            }

            if (name.Length > 255)
            {
                throw new ArgumentOutOfRangeException("Имя не должно быть больше 255 символов.", nameof(name));
            }
            Id = id;
            Name = name;
            TimeList = new List<Time>();
            WorksList = new List<Batch>();
            CurrentLoad = 0;
        }


        public int GetProcessTimeById(int id)
        {
            if (TimeList != null)
                return (TimeList.Where(time => time.Nomenclature.Id == id).Select(time => time.OperationTime))
                    .FirstOrDefault();
            return 0;
        }

        public void AddWork(Batch batch)
        {
            var processTime = GetProcessTimeById(batch.Nomenclature.Id);
            if (processTime == 0) return;
            CurrentLoad += processTime;
            WorksList.Add(batch);
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
            return Id.ToString() + " - " + Name + " \n              " + string.Join("\n              ", TimeList);
        }

    }
}
