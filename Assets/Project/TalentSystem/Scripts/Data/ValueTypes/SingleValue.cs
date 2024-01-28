using System;
using Newtonsoft.Json;

namespace SkillSystem.SkillValues
{
    [Serializable]
    public class SingleValue : ISkillValue
    {
        public float Value;
        [JsonIgnore] public string ValueString => $"{Value}";
    }
}