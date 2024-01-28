using System.Collections.Generic;
using System.Linq;
using Project.TalentSystem.Scripts.Data.Serialization;
using Utilities;
using UnityEngine;

namespace SkillSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Talent Tree", menuName = "Skill System/New Talent Tree", order = 1)]
    public class TalentTreeObject : ScriptableObject, ISerializableObjectContainer
    {
        [SerializeReference, SelectableReferenceType(typeof(IScriptableTalentNode))]
        private List<IScriptableTalentNode> _talents;
        public List<IScriptableTalentNode> Talents => _talents;

        private void OnEnable()
        {
            _talentTree = null;
        }

        private TalentTree _talentTree;
        public TalentTree TalentTree
        {
            get
            {
                if (_talentTree != null) return _talentTree;
                
                _talentTree = new TalentTree();
                
                foreach (IScriptableTalentNode scriptableObjectNode in Talents)
                {
                    if (scriptableObjectNode == null)
                    {
                        _talentTree.Nodes.Add(new("", 0, null, null));
                        continue;
                    }
                    TalentTree.TalentNode treeNode = new(scriptableObjectNode.Name, scriptableObjectNode.PointsRequirement, scriptableObjectNode.Prerequisite != null ? scriptableObjectNode.Prerequisite.Talent : null, scriptableObjectNode.Talents?.Select(x => x.Talent).ToList());
                    _talentTree.Nodes.Add(treeNode);
                }

                return _talentTree;
            }
            set
            {
                _talentTree = value;
                throw new System.NotImplementedException();
            }
        }

        object ISerializableObjectContainer.InnerObject
        {
            get => TalentTree;
            set => TalentTree = (TalentTree)value;
        }
    }
}