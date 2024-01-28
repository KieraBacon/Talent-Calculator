using System;
using System.Linq;
using System.Collections.Generic;

namespace SkillSystem
{
    [Serializable]
    public class CharacterInstance
    {
        private string _name;
        private readonly CharacterClass _characterClass;
        public CharacterClass CharacterClass => _characterClass;
        private List<TalentTreeInstance> _talentTreeInstances;
        public IReadOnlyList<TalentTreeInstance> TalentTreeInstances => _talentTreeInstances.AsReadOnly();
        public Action<CharacterInstance> OnInvestmentChanged;
        public int Investment => _talentTreeInstances.Sum(x => x.Investment);
        public int MaxInvestment => _characterClass.MaxTalentInvestment;
        public bool HasMaxInvestment => Investment >= MaxInvestment;
        public List<Talent> UnlockedTalents => TalentTreeInstances.SelectMany(x => x.UnlockedTalents).ToList();
        public bool HasTalentUnlocked(Talent talent) => talent == null || UnlockedTalents.Contains(talent);

        public CharacterInstance(CharacterClass characterClass)
        {
            _characterClass = characterClass;
            _talentTreeInstances = new();

            if (_characterClass == null) return;
            _name = _characterClass.Name;
            
            if (_characterClass.TalentTrees == null) return;
            foreach (TalentTree talentTree in _characterClass.TalentTrees)
            {
                TalentTreeInstance talentTreeInstance = new(this, talentTree);
                talentTreeInstance.OnInvestmentChanged += HandleTalentTreeInstanceInvestmentChange;
                _talentTreeInstances.Add(talentTreeInstance);
            }
        }

        private void HandleTalentTreeInstanceInvestmentChange(TalentTreeInstance obj)
        {
            foreach (TalentTreeInstance talentTreeInstance in TalentTreeInstances) talentTreeInstance.OnInvestmentChanged -= HandleTalentTreeInstanceInvestmentChange;
            OnInvestmentChanged?.Invoke(this);
            foreach (TalentTreeInstance talentTreeInstance in TalentTreeInstances) talentTreeInstance.OnInvestmentChanged += HandleTalentTreeInstanceInvestmentChange;
        }
    }
}