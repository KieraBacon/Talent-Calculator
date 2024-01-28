using Newtonsoft.Json;

namespace Project.TalentSystem.Scripts.Data.Serialization
{
    [JsonConverter(typeof(ISerializableObjectContainerConverter))]
    public interface ISerializableObjectContainer
    {
        internal object InnerObject { get; set; }
    }
}