using System;
using System.Collections.Generic;

namespace SkillSystem
{
    [Serializable]
    public class CharacterClass
    {
        public string Name;
        public int MaxTalentInvestment;
        public List<TalentTree> TalentTrees;
    }
}