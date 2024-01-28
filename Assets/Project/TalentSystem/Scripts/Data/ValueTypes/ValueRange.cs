using System;
using Newtonsoft.Json;
using UnityEngine;

namespace SkillSystem.SkillValues
{
    [Serializable]
    public class ValueRange : ISkillValue, ISerializationCallbackReceiver
    {
        [JsonIgnore] [SerializeField] private float _firstValue;
        [JsonIgnore] [SerializeField] private float _secondValue;
        
        public float Min
        {
            get
            {
                Validate();
                return _firstValue;
            }
            set
            {
                _firstValue = value;
                if (_secondValue < _firstValue) _secondValue = value;
            }
        }

        public float Max
        {
            get
            {
                Validate();
                return _secondValue;
            }
            set
            {
                _secondValue = value;
                if (_firstValue > _secondValue) _firstValue = value;
            }
        }

        [JsonIgnore] public string ValueString => $"{Min} to {Max}";
        
        public void OnBeforeSerialize() => Validate();
        public void OnAfterDeserialize() => Validate();
        public void Validate()
        {
            if (_firstValue > _secondValue) (_firstValue, _secondValue) = (_secondValue, _firstValue);
        }
    }
}