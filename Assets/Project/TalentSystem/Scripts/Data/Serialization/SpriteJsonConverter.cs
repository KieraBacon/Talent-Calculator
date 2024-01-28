using System;
using Newtonsoft.Json;
using UnityEngine;

namespace SkillSystem
{
    public class SpriteJsonConverter : JsonConverter<Sprite>
    {
        public override void WriteJson(JsonWriter writer, Sprite value, JsonSerializer serializer)
        {
            writer.WriteValue(value != null ? value.name : "");
        }

        public override Sprite ReadJson(JsonReader reader, Type objectType, Sprite existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string name = (string)reader.Value;
            return string.IsNullOrWhiteSpace(name) ? Resources.Load<Sprite>(name) : null;
        }
    }
}