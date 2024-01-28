using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem.ScriptableObjects
{
    [Serializable] public class MultiTalentDependentNode : IScriptableTalentNode
    {
        [SerializeField] private string _name;
        public string Name => _name;
        [SerializeField] private int _pointsRequirement;
        public int PointsRequirement => _pointsRequirement;
        [SerializeField] private TalentObject _prerequisite;
        public TalentObject Prerequisite => _prerequisite;
        [SerializeField] private List<TalentObject> _talents;
        public List<TalentObject> Talents => _talents;
    }
}