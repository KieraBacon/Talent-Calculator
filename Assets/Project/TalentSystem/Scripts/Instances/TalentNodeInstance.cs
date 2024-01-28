using System;
using UnityEngine;
using Utilities;

namespace SkillSystem
{
    [Serializable] public class TalentNodeInstance
    {
        public enum AvailabilityStates
        {
            Unavailable = 0,
            Available = 1,
            FullyTaken = 2,
        }

        private readonly TalentTreeInstance _talentTreeInstance;
        public TalentTreeInstance TalentTreeInstance => _talentTreeInstance;
        private TalentTree.TalentNode _talentNode;
        public TalentTree.TalentNode TalentNode => _talentNode;
        public Action<TalentNodeInstance> OnInvestmentChanged;
        private int _investment;
        public int Investment => _investment;
        public int MaxInvestment => TalentNode.MaxInvestment;
        public bool HasMaxInvestment => Investment >= MaxInvestment;
        public bool HasAnyInvestment => Investment > 0;
        public int AvailabilityThreshold => TalentNode.PointsRequirement;
        public bool OverAvailabilityThreshold => TalentNode != null && TalentTreeInstance.Investment - Investment >= AvailabilityThreshold;
        public bool HasPrerequisiteUnlocked => TalentTreeInstance.CharacterInstance.HasTalentUnlocked(TalentNode.Prerequisite);
        public bool UnderInvestmentLimit => !TalentTreeInstance.CharacterInstance.HasMaxInvestment;
        public AvailabilityStates AvailabilityState => 
            !OverAvailabilityThreshold || !HasPrerequisiteUnlocked || (!UnderInvestmentLimit && !HasAnyInvestment) ? AvailabilityStates.Unavailable : 
            !HasMaxInvestment ? AvailabilityStates.Available : 
            AvailabilityStates.FullyTaken;
        public int FirstRank => 1;
        public int LastRank => MaxInvestment;
        public int CurrentRank => Investment > -1 && Investment <= MaxInvestment ? Investment : -1;
        public int NextRank => Investment + 1 > -1 && Investment + 1 <= MaxInvestment ? Investment + 1 : -1;
        public int DisplayRank => CurrentRank < FirstRank ? FirstRank : CurrentRank > LastRank ? LastRank : CurrentRank;
        public Talent FirstRankTalent => TalentNode[FirstRank - 1];
        public Talent LastRankTalent => TalentNode[LastRank - 1];
        public Talent CurrentRankTalent => CurrentRank > -1 ? TalentNode[CurrentRank - 1] : null;
        public Talent NextRankTalent => NextRank > -1 ? TalentNode[NextRank - 1] : null;
        public Talent DisplayRankTalent => TalentNode[DisplayRank - 1];

        public string Tooltip
        {
            get
            {
                Color gold = new Color(0.8901961f, 0.7294118f, 0.01176471f);

                string RankInvestment() =>
                    $"Rank: {Investment}";

                string Type() =>
                    "Talent".Colour(Color.gray);

                string NextRank() =>
                    "Next rank:";

                string Footer() =>
                    (Investment == MaxInvestment ? "Right-click to unlearn" : "Click to learn").Colour(Color.green);

                string result = "";
                if (Investment == 0)
                    result = FirstRankTalent.Name.SpanTo(RankInvestment()) + "\n" +
                             Type() + "\n" +
                             FirstRankTalent.FullDescription.Colour(gold) + "\n" +
                             "\n" +
                             Footer();
                else if (Investment == MaxInvestment)
                    result = LastRankTalent.Name.SpanTo(RankInvestment()) + "\n" +
                             Type() + "\n" +
                             LastRankTalent.FullDescription.Colour(gold) + "\n\n" +
                             "\n" +
                             Footer();
                else
                    result = CurrentRankTalent.Name.SpanTo(RankInvestment()) + "\n" +
                             Type() + "\n" +
                             CurrentRankTalent.FullDescription.Colour(gold) + "\n" +
                             "\n" +
                             NextRank() + "\n" +
                             NextRankTalent.FullDescription.Colour(gold) + "\n" +
                             "\n" +
                             Footer();
                return result;
            }
        }

        public TalentNodeInstance(TalentTreeInstance talentTreeInstance, TalentTree.TalentNode talentNode, int investment)
        {
            _talentTreeInstance = talentTreeInstance;
            _talentNode = talentNode;
            _investment = investment;
            _talentTreeInstance.OnInvestmentChanged += HandleTalentTreeInstanceInvestmentChange;
        }

        private void HandleTalentTreeInstanceInvestmentChange(TalentTreeInstance obj)
        {
            if (!HasPrerequisiteUnlocked || !OverAvailabilityThreshold)
            {
                Increment(-_investment);
            }

            OnInvestmentChanged?.Invoke(this);
        }

        public void Increment(int increment)
        {
            int pointsAvailable = TalentTreeInstance.CharacterInstance.MaxInvestment - TalentTreeInstance.CharacterInstance.Investment;
            int maxAvailable = Mathf.Min(MaxInvestment, Investment + pointsAvailable);
            int newInvestment = Mathf.Clamp(Investment + increment, 0, maxAvailable);
            _investment = newInvestment;
            OnInvestmentChanged?.Invoke(this);
        }
    }

    public static class TalentNodeInstanceExtensions
    {
        public static bool IsValid(this TalentNodeInstance node) =>
            node?.MaxInvestment > 0;
    }
}