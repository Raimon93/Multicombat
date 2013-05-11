namespace MultiCombat.Classes
{
    using MyTERA.Helpers;
    using System;

    public class Player
    {
        public static bool isCombat;
        public static bool isCombo;
        public static bool isHPRegen;
        public static bool isLong;
        public static bool isMPRegen;
        public static bool isPull;
        public static int level;
        private DateTime start;
        private uint startXP;

        public Player()
        {
            this.SetStartTime(DateTime.Now);
            this.SetStartXP(GetCurrentXP());
        }

        public static bool CanCast()
        {
            return !LocalPlayer.IsCasting;
        }

        public static bool CanMount()
        {
            return !LocalPlayer.IsMounted;
        }

        public static PlayerClass GetClass()
        {
            return (PlayerClass) LocalPlayer.S1PlayerStatController.Class;
        }

        public static string GetClassName()
        {
            return LocalPlayer.S1PlayerStatController.Class.ToString();
        }

        public static uint GetCurrentXP()
        {
            return LocalPlayer.S1PlayerStatController.CurrentXP;
        }

        public uint GetGainedXP()
        {
            uint num = GetCurrentXP() - this.GetStartXP();
            if (num == 0)
            {
                num = 1;
            }
            return num;
        }

        public double GetHoursBotted()
        {
            TimeSpan span = (TimeSpan) (DateTime.Now - this.GetStartTime());
            return span.TotalHours;
        }

        public static uint GetHP()
        {
            return LocalPlayer.S1PlayerStatController.Health;
        }

        public static uint GetHPPerc()
        {
            return LocalPlayer.S1PlayerStatController.HealthPercentage;
        }

        public static int GetLevel()
        {
            return LocalPlayer.S1PlayerStatController.Level;
        }

        private double getMinutesBotted()
        {
            TimeSpan span = (TimeSpan) (DateTime.Now - this.GetStartTime());
            return span.TotalMinutes;
        }

        public static uint GetMP()
        {
            return LocalPlayer.S1PlayerStatController.Mana;
        }

        public static uint GetMPPerc()
        {
            return LocalPlayer.S1PlayerStatController.ManaPercentage;
        }

        public static string GetName()
        {
            return LocalPlayer.S1PlayerStatController.Name;
        }

        public DateTime GetNextLevelIn()
        {
            return DateTime.Now.AddMinutes((double) (GetNextLevelRequiredXP() / (this.GetXPRateHour() / 60)));
        }

        public static uint GetNextLevelRequiredXP()
        {
            return (GetRequiredXP() - GetCurrentXP());
        }

        public static uint GetRequiredXP()
        {
            return LocalPlayer.S1PlayerStatController.RequiredXP;
        }

        public static uint GetStamina()
        {
            return LocalPlayer.S1UserGameDataController.Endurance;
        }

        public DateTime GetStartTime()
        {
            return this.start;
        }

        public uint GetStartXP()
        {
            return this.startXP;
        }

        public uint GetXPRateHour()
        {
            double hoursBotted = this.GetHoursBotted();
            if (hoursBotted == 0.0)
            {
                return 60;
            }
            if (this.GetGainedXP() == 0)
            {
                return 60;
            }
            return (uint) (((double) this.GetGainedXP()) / hoursBotted);
        }

        public static bool HasHPPerc(int hp)
        {
            return (GetHPPerc() >= hp);
        }

        public static bool HasMana(uint mp)
        {
            return (GetMP() >= mp);
        }

        public static bool HasMPPerc(int mp)
        {
            return (GetMPPerc() >= mp);
        }

        public static bool HasStamina(uint stamina)
        {
            return (GetStamina() >= stamina);
        }

        public static bool IsLongRange()
        {
            switch (GetClass())
            {
                case PlayerClass.Sorcerer:
                case PlayerClass.Archer:
                case PlayerClass.Priest:
                case PlayerClass.Mystic:
                    return true;
            }
            return false;
        }

        public void SetStartTime(DateTime start)
        {
            this.start = start;
        }

        public void SetStartXP(uint start)
        {
            this.startXP = start;
        }

        public static void SwitchToCombat()
        {
            isCombo = false;
            isPull = false;
            isLong = false;
            isHPRegen = false;
            isMPRegen = false;
            isCombat = true;
        }

        public static void SwitchToCombo()
        {
            isCombo = true;
            isPull = false;
            isLong = false;
            isHPRegen = false;
            isMPRegen = false;
            isCombat = false;
        }

        public static void SwitchToHpRegen()
        {
            isCombo = false;
            isPull = false;
            isLong = false;
            isHPRegen = true;
            isMPRegen = false;
            isCombat = false;
        }

        public static void SwitchToLong()
        {
            isCombo = false;
            isPull = false;
            isLong = true;
            isHPRegen = false;
            isMPRegen = false;
            isCombat = false;
        }

        public static void SwitchToMpRegen()
        {
            isCombo = false;
            isPull = false;
            isLong = false;
            isHPRegen = false;
            isMPRegen = true;
            isCombat = false;
        }

        public static void SwitchToPull()
        {
            isCombo = false;
            isPull = true;
            isLong = false;
            isHPRegen = false;
            isMPRegen = false;
            isCombat = false;
        }

        public static PlayerClass Class
        {
            get
            {
                return (PlayerClass) LocalPlayer.S1PlayerStatController.Class;
            }
        }

        public static string ClassName
        {
            get
            {
                return ((PlayerClass) LocalPlayer.S1PlayerStatController.Class).ToString();
            }
        }

        public enum MovementFlag : uint
        {
            Backward = 2,
            Forward = 1
        }

        public enum PlayerClass : uint
        {
            Archer = 6,
            Berserker = 4,
            Lancer = 2,
            Mystic = 8,
            Priest = 7,
            Slayer = 3,
            Sorcerer = 5,
            Warrior = 1
        }
    }
}

