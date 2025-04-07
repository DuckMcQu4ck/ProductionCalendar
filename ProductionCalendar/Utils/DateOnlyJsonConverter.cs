using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductionCalendar.Utils
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private readonly string _serializationFormat;

        // Если нужно фиксированное форматирование, можно захардкодить:
        public DateOnlyJsonConverter(string serializationFormat = "dd.MM.yyyy")
        {
            _serializationFormat = serializationFormat;
        }

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();

            if (!string.IsNullOrEmpty(value) && DateOnly.TryParseExact(value, _serializationFormat, out var date))
            {
                return date;
            }
            throw new JsonException($"Невозможно десериализовать {value} в DateOnly.");
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_serializationFormat));
        }
    }
}
