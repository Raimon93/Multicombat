namespace MultiCombat.Classes
{
    using System;

    [Serializable]
    public class Skill
    {
        public int CastTimeSeconds;
        public bool DoubleAction;
        public int MaxRange = 15;
        public int MinRange;
        public string Name = string.Empty;
        public uint SkillId;
    }
}

