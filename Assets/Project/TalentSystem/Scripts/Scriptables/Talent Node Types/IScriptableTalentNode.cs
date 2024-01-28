using System.Collections.Generic;

namespace SkillSystem.ScriptableObjects
{
    public interface IScriptableTalentNode
    {
        string Name { get; }
        int PointsRequirement { get; }
        TalentObject Prerequisite { get; }
        List<TalentObject> Talents { get; }
    }
}