using System;
using Newtonsoft.Json;

namespace ExamEdrian.Services
{
    public class NoNullableIntConverter : JsonConverter
    {
        public NoNullableIntConverter()
        {
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if(reader.Value == null)
            {
                return default(int);
            }

            return Convert.ToInt32(reader.Value);

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
