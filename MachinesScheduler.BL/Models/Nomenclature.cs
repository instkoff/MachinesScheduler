using Newtonsoft.Json;

namespace MachinesScheduler.BL.Models
{
    /// <summary>
    /// Класс номенклатуры
    /// </summary>
    public class Nomenclature
    {
        //Id
        [JsonProperty("Column0")]
        public int Id { get; set; }
        //Название номенклатуры
        [JsonProperty("Column1")]
        public string Name { get; set; }

        public override string ToString()
        {
            return Id + " " + Name;
        }
    }
}
