using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExcelDataReader;
using MachinesScheduler.BL.Interfaces;
using MachinesScheduler.BL.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using Formatting = Newtonsoft.Json.Formatting;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace MachinesScheduler.BL.Services
{
    /// <summary>
    /// Класс для работы с экселем
    /// </summary>
    public class ImportFromExcelDataService : IImportDataService
    {
        private readonly ILogger<ImportFromExcelDataService> _logger;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="logger">Получаем логгер для логирования исключений</param>
        public ImportFromExcelDataService(ILogger<ImportFromExcelDataService> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Класс для конвертирования дробных значений Id из книги экселя в int.
        /// (Почему-то они приходят в ввиде 0.0; 1.0 и тд)
        /// </summary>
        private class CustomIntConverter : JsonConverter
        {
            private readonly string _filePath;
            private readonly ILogger<ImportFromExcelDataService> _logger;
            public CustomIntConverter(string path, ILogger<ImportFromExcelDataService> logger)
            {
                _filePath = path;
                _logger = logger;
            }
            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(int));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JValue jsonValue = serializer.Deserialize<JValue>(reader);

                if (jsonValue != null && jsonValue.Type == JTokenType.Float)
                {
                    return (int)Math.Round(jsonValue.Value<double>());
                }

                if (jsonValue != null && jsonValue.Type == JTokenType.Integer)
                {
                    return jsonValue.Value<int>();
                }

                _logger.LogError($"Не удалось сконвертировать значение \"{jsonValue}\" в файле {_filePath}");
                return jsonValue;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Метод для импорта из книги эксель
        /// </summary>
        /// <typeparam name="T">Т будет экземпляром конкретного объекта.</typeparam>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Список объектов.</returns>
        public IEnumerable<T> Import<T>(string path) where T : class
        {
            try
            {
                //Пытаемся открыть файл
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    //Считываем файл
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        //получаем первыю таблицу
                        var dataTable = reader.AsDataSet().Tables[0];
                        //Удаляем первую строку с шапкой
                        dataTable.Rows.RemoveAt(0);
                        //Сериализуем остальное в JSON формат
                        var json = JsonConvert.SerializeObject(dataTable, Formatting.Indented);
                        //Устанавливаем настройки десериализации
                        var settings = new JsonSerializerSettings
                        {
                            Converters = new List<JsonConverter> {new CustomIntConverter(path, _logger)}
                        };
                        //Десериализуем из JSON в объекты. Возвращаем список объектов.

                        return JsonConvert.DeserializeObject<IEnumerable<T>>(json, settings);
                    }
                }
            }
            //Обработка различных исключений.
            catch (IOException e)
            {
                _logger.LogError(e, $"Не могу получить доступ к файлу {path}. Возможно файл открыт в другой программе.");
                Console.ReadKey();
                throw;
            }
            catch (FormatException e)
            {
                _logger.LogError(e.Message);
                Console.ReadKey();
                throw;
            }
            catch (JsonSerializationException e)
            {
                _logger.LogError(e.Message);
                Console.ReadKey();
                throw;
            }
        }
    }
}
