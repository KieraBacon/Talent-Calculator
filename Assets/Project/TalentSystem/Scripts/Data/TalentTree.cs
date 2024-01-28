using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace SkillSystem
{
    [Serializable]
    public class TalentTree
    {
        [Serializable]
        public class TalentNode
        {
            [SerializeField] private string _name;
            public string Name => _name;
            [SerializeField] private int _pointsRequirement;
            public int PointsRequirement => _pointsRequirement;
            [SerializeField] private Talent _prerequisite;
            public Talent Prerequisite => _prerequisite;
            [SerializeField] private List<Talent> _talents;
            public List<Talent> Talents => _talents;

            [JsonIgnore]
            public int MaxInvestment => Talents?.Count ?? 0;

            public TalentNode(string name, int pointsRequirement, Talent prerequisite, List<Talent> talents)
            {
                _name = name;
                _pointsRequirement = pointsRequirement;
                _prerequisite = prerequisite;
                _talents = talents;
            }
            
            [JsonIgnore]
            public Talent this[int rank]
            {
                get => Talents[rank];
                set => Talents[rank] = value;
            }

            public bool EquivalentTo(TalentNode other)
            {
                return other != null && other._name == _name && other._pointsRequirement == _pointsRequirement && other.Prerequisite == Prerequisite && other.Talents == Talents;
            }
        }

        [SerializeField] private List<TalentNode> _nodes;
        public List<TalentNode> Nodes => _nodes;

        public TalentTree(List<TalentNode> nodes = null)
        {
            _nodes = nodes ?? new List<TalentNode>();
        }
    }
}