namespace MultiCombat.Classes
{
    using System;

    [Serializable]
    public class Heal
    {
        public string Name = string.Empty;
        public int PlayerHealthPct = 50;
        public uint SkillId;
    }
}

