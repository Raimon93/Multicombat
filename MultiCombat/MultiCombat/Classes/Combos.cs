namespace MultiCombat.Classes
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Combos
    {
        public string ComboName;
        public List<Skill> ComboSkills = new List<Skill>();
    }
}

