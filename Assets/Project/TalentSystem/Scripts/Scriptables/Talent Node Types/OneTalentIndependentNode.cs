using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem.ScriptableObjects
{
    [Serializable] public class OneTalentIndependentNode : IScriptableTalentNode
    {
        public string Name => _talent.Talent.Name;
        [SerializeField] private int _pointsRequirement;
        public int PointsRequirement => _pointsRequirement;
        public TalentObject Prerequisite => null;
        [SerializeField] private TalentObject _talent;
        public List<TalentObject> Talents => new() { _talent };
    }
}