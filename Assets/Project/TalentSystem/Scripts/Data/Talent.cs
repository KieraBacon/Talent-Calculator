using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Utilities;
using Newtonsoft.Json;
using UnityEngine;
using SkillSystem.SkillValues;

namespace SkillSystem
{
    [Serializable] public class Talent
    {
        public string Name;

        [JsonConverter(typeof(SpriteJsonConverter))]
        public Sprite Sprite;

        public string Description;

        [SerializeReference, SelectableReferenceType(typeof(ISkillValue))]
        public List<ISkillValue> Values;

        [JsonIgnore] public string FullDescription
        {
            get
            {
                string result = Description;
                List<ISkillValue> values = Values;
                result = Regex.Replace(result, @"\{(\d+)\}", match =>
                    match.Groups.Count > 0 && int.TryParse(match.Groups[1].Value, out int number) && values.Count > number ?
                        $"{values[number]?.ValueString ?? "NaN"}" :
                        "NaN");

                return result;
            }
        }
    }
}