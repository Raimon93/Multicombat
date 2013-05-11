namespace MultiCombat.Classes
{
    using System;

    [Serializable]
    public class Buff
    {
        public uint BuffId;
        public string BuffName = string.Empty;
        public bool isToggle;
        public uint SkillId;
        public string SkillName = string.Empty;
    }
}

