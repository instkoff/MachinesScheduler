using Newtonsoft.Json;

namespace MachinesScheduler.BL.Models
{
    public class Nomenclature
    {
        [JsonProperty("Column0")]
        public int Id { get; set; }
        [JsonProperty("Column1")]
        public string Name { get; set; }

        public override string ToString()
        {
            return Id + " " + Name;
        }
    }
}
