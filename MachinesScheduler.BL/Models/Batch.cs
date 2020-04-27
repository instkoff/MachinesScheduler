using Newtonsoft.Json;

namespace MachinesScheduler.BL.Models
{
    public class Batch
    {
        [JsonProperty("Column0")]
        public int Id { get; set; }

        [JsonProperty("Column1")] 
        public int NomenclatureId { get; set; }
        public Nomenclature Nomenclature { get; set; }

        public override string ToString()
        {
            return Nomenclature.Name;
        }
    }
}
