namespace MultiCombat.Classes
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Settings
    {
        public List<Skill> Buffs = new List<Skill>();
        public bool CastFinalCombo;
        public List<Skill> castTimeSkills = new List<Skill>();
        public int chanceCharging = 90;
        public int chanceDefault = 20;
        public int CloseRange = 4;
        public List<Skill> CombatSkills = new List<Skill>();
        public List<Skill> ComboSkills = new List<Skill>();
        public List<Skill> DoubleCastSkills = new List<Skill>();
        public List<Skill> HealSkills = new List<Skill>();
        public bool isBandage = true;
        public bool isDetectAggro = true;
        public bool isDetectPlayer = true;
        public bool isHPItem = true;
        public bool isLogCast = true;
        public bool isLogChain = true;
        public bool isLogXP = true;
        public bool isMPItem = true;
        public bool isStamina = true;
        public bool isStrafe;
        public bool isStrafeCharge = true;
        public bool isStrafePlayer = true;
        public bool isWait4Ms;
        public int LongRange = 10;
        public List<Skill> LongRangeSkills = new List<Skill>();
        public MultiCombat.Classes.Loot Loot = new MultiCombat.Classes.Loot();
        public int maxAggroDist = 15;
        public int maxPlayerDist = 500;
        public int maxStrafeTime = 800;
        public int MinHpPerc = 40;
        public int MinMpPerc = 40;
        public int minStamina = 20;
        public int minStrafeTime = 600;
        public List<Skill> MPRegenSkills = new List<Skill>();
        public List<Combos> NewComboSkills = new List<Combos>();
        public List<Skill> NoChainSkills = new List<Skill>();
        public int PullRange = 0x11;
        public List<Skill> PullSkills = new List<Skill>();
        public bool useCampfire = true;
        public bool usePanacea = true;
        public int wait4MS = 0x3e8;
        public List<Skill> waitChainSkills = new List<Skill>();
    }
}

