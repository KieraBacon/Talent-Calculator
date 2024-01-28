using System;
using Newtonsoft.Json;
using UnityEngine;
using Utilities;

namespace SkillSystem.SkillValues
{
    [Serializable]
    public class School : ISkillValue, ISerializationCallbackReceiver
    {
        [JsonIgnore, SerializeField] private ScriptableObjects.SchoolObject _schoolObject;
        [JsonProperty("Name"), SerializeField, ReadOnly] private string _name;
        [JsonIgnore] public string ValueString => _name;

        public void OnBeforeSerialize()
        {
            Validate();
        }

        public void OnAfterDeserialize()
        {
            Validate();
        }

        public void Validate()
        {
            if (_schoolObject != null && _name != _schoolObject.Name)
                _name = _schoolObject.Name;
        }
    }
}