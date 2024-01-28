using Newtonsoft.Json;
using UnityEngine;

namespace SkillSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New School", menuName = "Skill System/New School", order = 3)]
    public class SchoolObject : ScriptableObject
    {
        [JsonProperty("Name")] [SerializeField] private string _name;
        [JsonIgnore] public string Name => _name;
    }
}