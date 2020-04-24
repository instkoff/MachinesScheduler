using System;

namespace MachinesScheduler.BL.Models
{
    public class Nomenclature
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Nomenclature()
        {
            
        }

        public Nomenclature(int id, string name)
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

        }
    }
}
