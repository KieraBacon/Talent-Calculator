using Newtonsoft.Json;

namespace SkillSystem.SkillValues
{
    [JsonConverter(typeof(ISkillValueConverter))]
    public interface ISkillValue
    {
        public string ValueString { get; }
        public void Validate() {}
    }
}