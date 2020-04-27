using System;
using System.Collections.Generic;
using System.IO;
using ExcelDataReader;
using MachinesScheduler.BL.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace MachinesScheduler.BL.Services
{
    public class ExcelDataService : ILoadDataService
    {
        public IList<T> Load<T>(string path) where T : class
        {
            try
            {
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var dataTable = reader.AsDataSet().Tables[0];
                        dataTable.Rows.RemoveAt(0);
                        var json = JsonConvert.SerializeObject(dataTable, Formatting.Indented);
                        JsonSerializerSettings settings = new JsonSerializerSettings
                        {
                            Converters = new List<JsonConverter> { new CustomIntConverter() }
                        };
                        return JsonConvert.DeserializeObject<IList<T>>(json,settings);

                    }
                }

            }
            //ToDO Изменить exception
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        private class CustomIntConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(int));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JValue jsonValue = serializer.Deserialize<JValue>(reader);

                if (jsonValue.Type == JTokenType.Float)
                {
                    return (int)Math.Round(jsonValue.Value<double>());
                }
                else if (jsonValue.Type == JTokenType.Integer)
                {
                    return jsonValue.Value<int>();
                }

                throw new FormatException();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }

}
