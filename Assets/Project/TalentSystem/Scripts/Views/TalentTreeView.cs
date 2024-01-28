using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace SkillSystem.UI
{
    public class TalentTreeView : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _layoutRoot;
        [SerializeField] private int _columns;
        private TalentTreeInstance _talentTreeInstance;
        public TalentTreeInstance TalentTreeInstance
        {
            get => _talentTreeInstance;
            set => _talentTreeInstance = value;
        }

        private List<TalentNodeView> _activeList = new();

        private void Start()
        {
            UpdateInformation();
            _layoutRoot.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _layoutRoot.constraintCount = _columns;
        }

        private void UpdateInformation()
        {
            Pool<TalentNodeView> pool = PoolManager.Instance.Get<TalentNodeView>("Talent Node View");
            pool.Release(_activeList.ToArray());
            
            foreach (TalentNodeInstance talentInstance in TalentTreeInstance.TalentNodesInstances)
            {
                TalentNodeView talentNodeView = pool.Get(_layoutRoot.transform);
                _activeList.Add(talentNodeView);
                talentNodeView.TalentNodeInstance = talentInstance;
            }
        }
    }
}