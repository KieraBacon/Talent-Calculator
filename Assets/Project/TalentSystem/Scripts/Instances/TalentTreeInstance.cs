using System;
using System.Linq;
using System.Collections.Generic;

namespace SkillSystem
{
    [Serializable]
    public class TalentTreeInstance
    {
        private readonly CharacterInstance _characterInstance;
        public CharacterInstance CharacterInstance => _characterInstance;
        private readonly TalentTree _talentTree;
        public TalentTree TalentTree => _talentTree;
        public Action<TalentTreeInstance> OnInvestmentChanged;
        private int _lastInvestment;
        public int Investment => TalentNodesInstances.Sum(x => x.Investment);
        private readonly List<TalentNodeInstance> _talentNodesInstances;
        public IReadOnlyList<TalentNodeInstance> TalentNodesInstances => _talentNodesInstances?.AsReadOnly();
        public List<Talent> UnlockedTalents => TalentNodesInstances?.SelectMany(x => x?.TalentNode?.Talents?.GetRange(0, x.Investment) ?? new List<Talent>()).ToList();

        public TalentTreeInstance(CharacterInstance characterInstance, TalentTree talentTree)
        {
            _characterInstance = characterInstance;
            _characterInstance.OnInvestmentChanged += HandleCharacterInstanceInvestmentChange;
            _talentTree = talentTree;
            _talentNodesInstances = new();

            if (TalentTree?.Nodes == null) return;
            foreach (TalentTree.TalentNode talentNode in TalentTree.Nodes)
            {
                TalentNodeInstance talentNodeInstance = new(this, talentNode, 0);
                _talentNodesInstances.Add(talentNodeInstance);
                talentNodeInstance.OnInvestmentChanged += HandleTalentInstanceInvestmentChange;
            }
        }

        private void HandleCharacterInstanceInvestmentChange(CharacterInstance obj)
        {
            foreach (TalentNodeInstance talentInstance in TalentNodesInstances) talentInstance.OnInvestmentChanged -= HandleTalentInstanceInvestmentChange;
            OnInvestmentChanged?.Invoke(this);
            foreach (TalentNodeInstance talentInstance in TalentNodesInstances) talentInstance.OnInvestmentChanged += HandleTalentInstanceInvestmentChange;
        }

        private void HandleTalentInstanceInvestmentChange(TalentNodeInstance talentNodeInstance)
        {
            UpdateInvestment();
        }

        private void UpdateInvestment()
        {
            int newInvestment = TalentNodesInstances.Sum(x => x.Investment);
            if (newInvestment == _lastInvestment) return;

            _lastInvestment = newInvestment;

            foreach (TalentNodeInstance talentInstance in TalentNodesInstances) talentInstance.OnInvestmentChanged -= HandleTalentInstanceInvestmentChange;
            OnInvestmentChanged?.Invoke(this);
            foreach (TalentNodeInstance talentInstance in TalentNodesInstances) talentInstance.OnInvestmentChanged += HandleTalentInstanceInvestmentChange;
            UpdateInvestment();
        }
    }
}