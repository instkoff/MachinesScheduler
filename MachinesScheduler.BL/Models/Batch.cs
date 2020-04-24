using System;

namespace MachinesScheduler.BL.Models
{
    public class Batch
    {
        public int Id { get; }
        public Nomenclature Nomenclature { get; }

        public Batch()
        {
            
        }

        public Batch(int id, Nomenclature nomenclature)
        {
            if (id < 0)
                throw new ArgumentException("Id не может быть меньше нуля.", nameof(id));
            if(nomenclature == null)
                throw new ArgumentNullException("Номенклатура должна быть заполнена.", nameof(nomenclature));

            Id = id;
            Nomenclature = nomenclature;
        }

        public override string ToString()
        {
            return Id + " " + Nomenclature.Name;
        }
    }
}
