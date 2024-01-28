using System.Collections.Generic;
using System.Linq;
using Project.TalentSystem.Scripts.Data.Serialization;
using UnityEngine;

namespace SkillSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Character Class", menuName = "Skill System/New Character Class", order = 2)]
    public class CharacterClassObject : ScriptableObject, ISerializableObjectContainer
    {
        public int MaxTalentInvestment;
        public List<TalentTreeObject> TalentTrees;

        private void OnEnable()
        {
            _characterClass = null;
        }

        private CharacterClass _characterClass;
        public CharacterClass CharacterClass
        {
            get
            {
                if (_characterClass != null) return _characterClass;

                _characterClass = new CharacterClass();
                _characterClass.Name = name;
                _characterClass.MaxTalentInvestment = MaxTalentInvestment;
                _characterClass.TalentTrees = TalentTrees.Select(x => x.TalentTree).ToList();
                return _characterClass;
            }
            set
            {
                _characterClass = value;
                throw new System.NotImplementedException();
            }
        }

        object ISerializableObjectContainer.InnerObject
        {
            get => CharacterClass;
            set => CharacterClass = (CharacterClass)value;
        }
    }
}