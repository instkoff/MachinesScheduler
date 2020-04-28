using Newtonsoft.Json;

namespace MachinesScheduler.BL.Models
{
    /// <summary>
    /// Класс партии материалов
    /// </summary>
    public class Batch
    {
        //Id
        [JsonProperty("Column0")]
        public int Id { get; set; }
        //Id связанной номенклатуры
        [JsonProperty("Column1")]
        public int NomenclatureId { get; set; }
        //Ссылка на конкретную номенклатуру
        public Nomenclature Nomenclature { get; set; }

        public override string ToString()
        {
            return Nomenclature.Name;
        }
    }
}
