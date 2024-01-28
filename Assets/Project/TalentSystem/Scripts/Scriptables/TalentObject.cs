using Project.TalentSystem.Scripts.Data.Serialization;
using UnityEngine;

namespace SkillSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Talent Node", menuName = "Skill System/New Talent Node", order = 0)]
    public class TalentObject : ScriptableObject, ISerializableObjectContainer
    {
        [SerializeField] private Talent _talent;
        public Talent Talent => _talent;

        public static TalentObject Instantiate(Talent talent)
        {
            TalentObject talentObject = CreateInstance<TalentObject>();
            talentObject._talent = talent;
            return talentObject;
        }

        object ISerializableObjectContainer.InnerObject
        {
            get => _talent;
            set => _talent = (Talent)value;
        }
    }
}