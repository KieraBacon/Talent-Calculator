using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SkillSystem.SkillValues
{
    public class ISkillValueConverter : JsonConverter<ISkillValue>
    {
        private object _lock = new object();
        private bool _canRead = true;
        public override bool CanRead => _canRead;
        private bool _canWrite = true;
        public override bool CanWrite => _canWrite;

        public override void WriteJson(JsonWriter writer, ISkillValue value, JsonSerializer serializer)
        {
            lock (_lock)
            {
                value.Validate();

                _canWrite = false;
                JObject jobj = JObject.FromObject(value);
                _canWrite = true;

                jobj.AddFirst(new JProperty("Type", value.GetType().Name));
                jobj.WriteTo(writer);
            }
        }

        public override ISkillValue ReadJson(JsonReader reader, Type objectType, ISkillValue existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            lock (_lock)
            {
                JToken token = JToken.ReadFrom(reader);
                string typeString = (string)token["Type"];
                if (string.IsNullOrWhiteSpace(typeString)) return null;

                Type type = Type.GetType("SkillSystem.SkillValues." + typeString);
                if (type == null) return null;

                _canRead = false;
                ISkillValue result = JsonConvert.DeserializeObject(token.ToString(), type) as ISkillValue;
                _canRead = true;

                result?.Validate();
                return result;
            }
        }
    }
}