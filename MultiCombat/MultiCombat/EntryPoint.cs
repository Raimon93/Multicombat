namespace MultiCombat
{
    using MultiCombat.Classes;
    using MyTERA.Classes;
    using MyTERA.GameData;
    /*using MyTERA.GameData.GeneratedStructures;*/
    using MyTERA.Helpers;
    using MyTERA.Helpers.AbnormalManager;
    using MyTERA.Helpers.InteractManager;
    using MyTERA.Helpers.ObjectManager;
    using MyTERA.Helpers.SkillsManager;
    using MyTERA.Resources;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    using ZurasBot;
    using ZurasBot.Addons;
    using ZurasBot.Classes;
    using ZurasBot.Functions;
    using ZurasBot.Logic;

    public class EntryPoint : ICombat
    {
        public int AggroDist = 5;
        public int aggroTarget;
        private static List<Structs.TERASkill> buffList = new List<Structs.TERASkill>();
        public int castTime;
        public int chanceCharging = 90;
        public int chanceDefault = 20;
        private static List<Structs.TERASkill> combatSkillList = new List<Structs.TERASkill>();
        private static List<Structs.TERASkill> comboSkillList = new List<Structs.TERASkill>();
        private static List<Structs.TERASkill> doubleCastSkillList = new List<Structs.TERASkill>();
        public List<ulong> forgetable = new List<ulong>();
        private static List<Structs.TERASkill> healSkillList = new List<Structs.TERASkill>();
        public bool isBandage = true;
        public bool isBotStopped = true;
        private bool isCampfireInterrupt;
        private bool isCampfireOn;
        private bool isDetectAggro = true;
        private bool isDetectPlayer = true;
        private bool isGoldOnly;
        public bool isHPItem = true;
        private bool isLogCast = true;
        private bool isLogChain = true;
        private bool isLogLoot = true;
        private bool isMCLoot = true;
        public bool isMPItem = true;
        public bool isMultiAggro = true;
        private bool isStamina = true;
        private bool isStrafe;
        public bool isStrafeCharge = true;
        public bool isStrafePlayer = true;
        private bool isWaitEnabled;
        private DataStructureAccess<Item> items = DataManager.Data.ItemData.Item;
        private Position lastCampfire = new Position();
        private MyTERA.Classes.Timeout LastHealTimeout = new MyTERA.Classes.Timeout(0x3e8);
        private Random leftRight = new Random();
        private int longDist;
        public bool longRangeClass;
        private static List<Structs.TERASkill> longRangeSkillList = new List<Structs.TERASkill>();
        public bool lootCommon = true;
        public bool lootEpic = true;
        public bool lootRare = true;
        public bool lootSuperior = true;
        public int maxAggro = 3;
        private double maxAggroDistance = 15.0;
        private uint maxCombatCostMP;
        private uint maxCostMP;
        private uint maxHealCostMP;
        private double maxLootDistance = 40.0;
        private double maxPlayerDistance = 400.0;
        private uint maxPullCostMP;
        public const int maxStamina = 120;
        public int maxStrafeTime = 800;
        private int meleeDist;
        private int minHpPerc;
        private int minMpPerc;
        private uint minStamina = 20;
        public int minStrafeTime = 600;
        private Random moveMS = new Random();
        private static List<Structs.TERASkill> mpRegenSkillList = new List<Structs.TERASkill>();
        public TERAObject nearestPlayer = TERAObject.Invalid;
        private static List<Structs.TERASkill> noChainSkillList = new List<Structs.TERASkill>();
        public Player plr;
        private int pullDist;
        private static List<Structs.TERASkill> pullSkillList = new List<Structs.TERASkill>();
        private Random rndStrafe = new Random();
        public uint sellValue;
        private MultiCombat.Settings settings;
        public uint startGold;
        private bool strafeLeft;
        public int strafeTime;
        private Thread Thread_CheckCombo;
        private bool useCampfire = true;
        private bool usePanacea = true;
        private int wait4MS;
        private static List<Structs.TERASkill> waitSkillList = new List<Structs.TERASkill>();

        private void AddGenericList(List<Skill> settingSkills, List<Structs.TERASkill> botSkills)
        {
            foreach (Skill skill in settingSkills)
            {
                botSkills.Add(MyTERA.Helpers.SkillsManager.SkillsManager.GetTERASkillById(skill.SkillId));
            }
        }

        private void AddNoMPSkills(List<Skill> settingSkills, List<Structs.TERASkill> mpSkills)
        {
            foreach (Skill skill in settingSkills)
            {
                Structs.TERASkill tERASkillById = MyTERA.Helpers.SkillsManager.SkillsManager.GetTERASkillById(skill.SkillId);
                if ((tERASkillById.GetCostMP() == 0) && !mpSkills.Contains(tERASkillById))
                {
                    mpSkills.Add(tERASkillById);
                }
            }
        }

        private void AddSkillsToBot()
        {
            pullSkillList.Clear();
            combatSkillList.Clear();
            comboSkillList.Clear();
            noChainSkillList.Clear();
            mpRegenSkillList.Clear();
            doubleCastSkillList.Clear();
            longRangeSkillList.Clear();
            healSkillList.Clear();
            waitSkillList.Clear();
            buffList.Clear();
            this.AddGenericList(Globals.Settings.PullSkills, pullSkillList);
            this.AddGenericList(Globals.Settings.CombatSkills, combatSkillList);
            this.AddGenericList(Globals.Settings.ComboSkills, comboSkillList);
            this.AddGenericList(Globals.Settings.NoChainSkills, noChainSkillList);
            this.AddGenericList(Globals.Settings.MPRegenSkills, mpRegenSkillList);
            this.AddGenericList(Globals.Settings.DoubleCastSkills, doubleCastSkillList);
            this.AddGenericList(Globals.Settings.LongRangeSkills, longRangeSkillList);
            this.AddGenericList(Globals.Settings.HealSkills, healSkillList);
            this.AddGenericList(Globals.Settings.waitChainSkills, waitSkillList);
            this.AddGenericList(Globals.Settings.Buffs, buffList);
            this.AddNoMPSkills(Globals.Settings.CombatSkills, mpRegenSkillList);
        }

        private bool CanCombat()
        {
            if (combatSkillList.Count == 0)
            {
                return false;
            }
            return true;
        }

        public bool CanCombo()
        {
            if (comboSkillList.Count == 0)
            {
                return false;
            }
            if (!Player.HasMana(this.GetMaxMPCost(comboSkillList)))
            {
                return false;
            }
            foreach (Structs.TERASkill skill in combatSkillList)
            {
                if (skill.IsValid && skill.OnCooldown())
                {
                    return false;
                }
            }
            Logger.WriteLine("Can Combo");
            return true;
        }

        private bool CanHeal()
        {
            if (healSkillList.Count != 0)
            {
                foreach (Structs.TERASkill skill in healSkillList)
                {
                    if ((skill.IsValid && !skill.OnCooldown()) && Player.HasMana(skill.GetCostMP()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CanLong()
        {
            if (longRangeSkillList.Count != 0)
            {
                foreach (Structs.TERASkill skill in longRangeSkillList)
                {
                    if ((skill.IsValid && !skill.OnCooldown()) && Player.HasMana(skill.GetCostMP()))
                    {
                        Logger.WriteLine("Can Long Range");
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CanPull()
        {
            if (pullSkillList.Count != 0)
            {
                foreach (Structs.TERASkill skill in pullSkillList)
                {
                    if ((skill.IsValid && !skill.OnCooldown()) && Player.HasMana(skill.GetCostMP()))
                    {
                        Logger.WriteLine("Can Pull");
                        return true;
                    }
                }
            }
            return false;
        }

        private void CheckBuffs()
        {
            foreach (Structs.TERASkill skill in buffList)
            {
                if (this.NearestAggro().IsValid)
                {
                    break;
                }
                if ((!isBuffActive(skill.Name) && skill.IsValid) && (!skill.OnCooldown() && Player.HasMana(skill.GetCostMP())))
                {
                    Mover.Stop();
                    Skills.CastSkillById(skill.Id, 0, false, null);
                }
            }
        }

        private void CheckFinalCombo()
        {
            while (true)
            {
                try
                {
                    uint skillId = Game.LocalTERAObject.S1SkillHotKeyController.ComboData__SkillId;
                    if (skillId != 0)
                    {
                        Structs.TERASkill tERASkillById = MyTERA.Helpers.SkillsManager.SkillsManager.GetTERASkillById(skillId);
                        if ((tERASkillById.IsValid && (!tERASkillById.OnCooldown() || doubleCastSkillList.Contains(tERASkillById))) && (Player.HasMana(tERASkillById.GetCostMP()) && !noChainSkillList.Contains(tERASkillById)))
                        {
                            TERAObject obj2 = this.NearestAggro();
                            if (!obj2.IsValid)
                            {
                                obj2 = this.NearestTarget();
                            }
                            if (obj2.IsValid && obj2.S1NpcGameDataController.IsAlive)
                            {
                                obj2.S1SkeletalMeshController.Position.Camera__Face();
                                Thread.Sleep(10);
                                if (waitSkillList.Contains(tERASkillById))
                                {
                                    if (this.isWaitEnabled)
                                    {
                                        Logger.WriteLine("Waiting for max " + this.wait4MS + " ms");
                                    }
                                    else
                                    {
                                        this.wait4MS = 0x7fffffff;
                                        Logger.WriteLine("Waiting until previous skill casted");
                                    }
                                    DateTime now = DateTime.Now;
                                    for (TimeSpan span = (TimeSpan) (DateTime.Now - now); (LocalPlayer.IsCasting || LocalPlayer.S1PlayerMoveController.IsCasting) && (span.TotalMilliseconds < this.wait4MS); span = (TimeSpan) (DateTime.Now - now))
                                    {
                                    }
                                }
                                obj2.S1SkeletalMeshController.Position.Camera__Face();
                                this.LogChain(tERASkillById.Name);
                                tERASkillById.CastAsCombo();
                                Thread.Sleep(200);
                            }
                        }
                    }
                }
                catch
                {
                }
                Thread.Sleep(300);
            }
        }

        private void CheckHP()
        {
            if (this.NeedHP())
            {
                if (this.isHPItem)
                {
                    Inventory.UseHealthtem();
                }
                Logger.WriteLine("Switch to HP Regen Mode. Hp (%): " + Player.GetHPPerc());
                if (this.CanHeal())
                {
                    Player.SwitchToHpRegen();
                }
            }
        }

        private void CheckMP()
        {
            if (this.NeedMP())
            {
                if (this.isMPItem)
                {
                    Inventory.UseManaItem();
                }
                Logger.WriteLine("Switch to MP Regen Mode. Mp (%): " + Player.GetMPPerc());
                Player.SwitchToMpRegen();
            }
        }

        public override bool Combat(TERAObject Object)
        {
            Player.SwitchToMpRegen();
            this.DoSkillsList(combatSkillList, this.meleeDist, Object);
            return true;
        }

        private Skill ConvertToSkill(Structs.TERASkill skill)
        {
            return new Skill { SkillId = skill.Id, Name = skill.Name, CastTimeSeconds = 0, DoubleAction = false, MaxRange = 0, MinRange = 0 };
        }

        private bool DoSkillsList(List<Structs.TERASkill> Skills, int Distance, TERAObject Object)
        {
            uint manaPercentage = LocalPlayer.S1PlayerStatController.ManaPercentage;
            uint mana = LocalPlayer.S1PlayerStatController.Mana;
            uint healthPercentage = LocalPlayer.S1PlayerStatController.HealthPercentage;
            uint health = LocalPlayer.S1PlayerStatController.Health;
            if (this.IsMultiAggro())
            {
                Object = this.NearestAggro();
            }
            Distance = (int) this.GetCombatDistance(this.GetDistanceFrom(Object));
            Skills = this.GetCombatSkillList();
            if (!Approach.ApproachTERAObject(Object, (double) Distance))
            {
                return false;
            }
            foreach (Structs.TERASkill skill2 in Skills)
            {
                Structs.TERASkill tERASkillById = MyTERA.Helpers.SkillsManager.SkillsManager.GetTERASkillById(skill2.Id);
                double distanceFrom = this.GetDistanceFrom(Object);
                try
                {
                    this.StayInRange(Object, Distance);
                }
                catch
                {
                }
                if (distanceFrom > Distance)
                {
                    Approach.ApproachTERAObject(Object, (double) Distance);
                }
                else if ((tERASkillById.IsValid && !tERASkillById.OnCooldown()) && Player.HasMana(tERASkillById.GetCostMP()))
                {
                    bool requireDoubleCast = doubleCastSkillList.Contains(tERASkillById);
                    Object.S1SkeletalMeshController.Position.Camera__Face();
                    Thread.Sleep(100);
                    this.GetDistanceFrom(Object);
                    if (requireDoubleCast)
                    {
                        Logger.WriteLine("Double Cast: " + tERASkillById.Name);
                    }
                    this.LogCast(tERASkillById.Name);
                    this.castTime = this.GetCastTimeById(tERASkillById.Id);
                    try
                    {
                        this.Strafe(Object);
                    }
                    catch
                    {
                    }
                    Skills.CastSkillById(tERASkillById.Id, this.castTime, requireDoubleCast, Object);
                    while (LocalPlayer.IsCasting || LocalPlayer.S1PlayerMoveController.IsCasting)
                    {
                    }
                    Thread.Sleep(1);
                    if (!Player.isCombo)
                    {
                        return true;
                    }
                }
            }
            return true;
        }

        private int GetCastTimeById(uint skillId)
        {
            foreach (Skill skill in Globals.Settings.castTimeSkills)
            {
                if (skillId == skill.SkillId)
                {
                    return skill.CastTimeSeconds;
                }
            }
            return 0;
        }

        private double GetCombatDistance(double d)
        {
            if (d > ((this.pullDist + this.longDist) / 2))
            {
                if (this.CanPull())
                {
                    Logger.WriteLine("Switch to Pull Mode.");
                    Player.SwitchToPull();
                    this.CheckHP();
                    return (double) this.pullDist;
                }
            }
            else if (d > ((this.meleeDist + this.longDist) / 2))
            {
                if (this.CanLong())
                {
                    Logger.WriteLine(string.Concat(new object[] { "Switch to Long Range Mode. (", this.longDist, " ", d, ")" }));
                    Player.SwitchToLong();
                    this.CheckHP();
                    this.CheckMP();
                    return (double) this.longDist;
                }
            }
            else
            {
                if (this.CanCombo())
                {
                    Logger.WriteLine("Switch to Combo Mode.");
                    Player.SwitchToCombo();
                    return (double) this.meleeDist;
                }
                Player.SwitchToCombat();
                Logger.WriteLine("Switch to Combat Mode.");
                this.CheckHP();
                this.CheckMP();
                return (double) this.meleeDist;
            }
            if (((d >= this.pullDist) || Player.isPull) && this.CanPull())
            {
                Logger.WriteLine("Switch to Pull Mode.");
                Player.SwitchToPull();
                return (double) this.pullDist;
            }
            if ((d >= this.longDist) && this.CanLong())
            {
                Logger.WriteLine(string.Concat(new object[] { "Switch to Long Range Mode. (", this.longDist, " ", d, ")" }));
                Player.SwitchToLong();
                return (double) this.longDist;
            }
            Player.SwitchToCombat();
            Logger.WriteLine("Switch to Combat Mode.");
            return (double) this.meleeDist;
        }

        public List<Structs.TERASkill> GetCombatSkillList()
        {
            if (Player.isCombo)
            {
                return comboSkillList;
            }
            if (!Player.isCombat)
            {
                if (Player.isHPRegen)
                {
                    return healSkillList;
                }
                if (Player.isMPRegen)
                {
                    return mpRegenSkillList;
                }
                if (Player.isPull)
                {
                    return pullSkillList;
                }
                if (Player.isLong)
                {
                    return longRangeSkillList;
                }
            }
            return combatSkillList;
        }

        private int GetCopper(uint totalValue)
        {
            return ((((int) totalValue) - (this.GetGold(totalValue) * 0x2710)) - (this.GetSilver(totalValue) * 100));
        }

        public double GetDistanceFrom(TERAObject o)
        {
            return o.S1SkeletalMeshController.Position.TERADistance3DFromPlayer;
        }

        private int GetGold(uint totalValue)
        {
            return (int) (totalValue / 0x2710);
        }

        private uint GetMaxMPCost(List<Structs.TERASkill> skillList)
        {
            uint costMP = 0;
            foreach (Structs.TERASkill skill in skillList)
            {
                if (skill.IsValid && (skill.GetCostMP() > costMP))
                {
                    costMP = skill.GetCostMP();
                }
            }
            return costMP;
        }

        private int GetSilver(uint totalValue)
        {
            return (int) ((totalValue - (this.GetGold(totalValue) * 0x2710)) / ((ulong) 100L));
        }

        public static bool isBuffActive(string name)
        {
            Dictionary<uint, Structs.TERAAbnormality> tERAAbnormalities = new Dictionary<uint, Structs.TERAAbnormality>(0);
            try
            {
                tERAAbnormalities = MyTERA.Helpers.AbnormalManager.AbnormalManager.GetTERAAbnormalities();
            }
            catch
            {
            }
            foreach (Structs.TERAAbnormality abnormality in tERAAbnormalities.Values)
            {
                if (abnormality.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isForgetable(TERAInteract itm)
        {
            foreach (ulong num in this.forgetable)
            {
                if (num == itm.S1VillagerVolume.TERAObject.GUID)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isInventoryFull()
        {
            return (Game.LocalTERAObject.S1InventoryController.FreeBagSlots == 0);
        }

        public bool isItemLootable(TERAInteract item)
        {
            for (int i = 0; i < this.items.Count; i++)
            {
                if (item.S1DropItemVolume.DropItemID == this.items[i].id)
                {
                    if (this.isGoldOnly && !item.S1DropItemVolume.IsGold)
                    {
                        return false;
                    }
                    if (((this.items[i].rank <= 0) || ((((this.items[i].rareGrade != 0) || !this.lootCommon) && ((this.items[i].rareGrade != 1) || !this.lootSuperior)) && (((this.items[i].rareGrade != 2) || !this.lootRare) && ((this.items[i].rareGrade != 3) || !this.lootEpic)))) && ((this.items[i].maxStack <= 1) && (this.items[i].rank != 0)))
                    {
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        public bool isItemStackable(TERAInteract item)
        {
            for (int i = 0; i < this.items.Count; i++)
            {
                if (item.S1DropItemVolume.DropItemID == this.items[i].id)
                {
                    return (this.items[i].maxStack > 1);
                }
            }
            return false;
        }

        public bool IsMultiAggro()
        {
            List<TERAObject> list = new List<TERAObject>(MyTERA.Helpers.ObjectManager.ObjectManager.GetTERAObjects().Values);
            uint num = 0;
            foreach (TERAObject obj2 in list)
            {
                if (((obj2.GUID != Game.LocalTERAObject.GUID) && obj2.S1NpcGameDataController.InCombat) && obj2.S1NpcGameDataController.IsAlive)
                {
                    num++;
                    if (num > 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isPlayerNearby()
        {
            if (this.isDetectPlayer)
            {
                List<TERAObject> list = new List<TERAObject>(MyTERA.Helpers.ObjectManager.ObjectManager.GetTERAObjects().Values);
                foreach (TERAObject obj2 in list)
                {
                    if (((obj2.GUID != Game.LocalTERAObject.GUID) && (obj2.S1SkeletalMeshController.Position.TERADistance3DFromPlayer < this.maxPlayerDistance)) && obj2.IsPlayer)
                    {
                        this.nearestPlayer = obj2;
                        return true;
                    }
                }
            }
            return false;
        }

        private void LoadSettingsToBot()
        {
            this.AddSkillsToBot();
            this.meleeDist = Globals.Settings.CloseRange;
            this.longDist = Globals.Settings.LongRange;
            this.pullDist = Globals.Settings.PullRange;
            this.minHpPerc = Globals.Settings.MinHpPerc;
            this.minMpPerc = Globals.Settings.MinMpPerc;
            this.wait4MS = Globals.Settings.wait4MS;
            this.isWaitEnabled = Globals.Settings.isWait4Ms;
            this.isLogCast = Globals.Settings.isLogCast;
            this.isLogChain = Globals.Settings.isLogChain;
            this.isLogLoot = Globals.Settings.isLogXP;
            this.isMCLoot = Globals.Settings.Loot.isMCLoot;
            this.isGoldOnly = Globals.Settings.Loot.isGoldOnly;
            this.maxLootDistance = Globals.Settings.Loot.maxLootDistance;
            this.maxAggroDistance = Globals.Settings.maxAggroDist;
            this.maxPlayerDistance = Globals.Settings.maxPlayerDist;
            this.isDetectAggro = Globals.Settings.isDetectAggro;
            this.isDetectPlayer = Globals.Settings.isDetectPlayer;
            this.isStrafe = Globals.Settings.isStrafe;
            this.isStamina = Globals.Settings.isStamina;
            this.useCampfire = Globals.Settings.useCampfire;
            this.usePanacea = Globals.Settings.usePanacea;
            this.minStamina = (uint) Globals.Settings.minStamina;
            this.isBandage = Globals.Settings.isBandage;
            this.isMPItem = Globals.Settings.isMPItem;
            this.isHPItem = Globals.Settings.isHPItem;
            this.isStrafeCharge = Globals.Settings.isStrafeCharge;
            this.isStrafePlayer = Globals.Settings.isStrafePlayer;
            this.minStrafeTime = Globals.Settings.minStrafeTime;
            this.maxStrafeTime = Globals.Settings.maxStrafeTime;
            this.chanceCharging = Globals.Settings.chanceCharging;
            this.chanceDefault = Globals.Settings.chanceDefault;
            this.lootCommon = Globals.Settings.Loot.lootCommon;
            this.lootEpic = Globals.Settings.Loot.lootEpic;
            this.lootRare = Globals.Settings.Loot.lootRare;
            this.lootSuperior = Globals.Settings.Loot.lootSuperior;
        }

        public void LogCast(string name)
        {
            if (this.isLogCast)
            {
                Logger.WriteLine("Casting " + name);
            }
        }

        public void LogChain(string name)
        {
            if (this.isLogChain)
            {
                Logger.WriteLine("Chain detected " + name);
            }
        }

        public void LogLootValue()
        {
            if (this.isLogLoot)
            {
                double num = 0.0;
                TimeSpan span = (TimeSpan) (DateTime.Now - this.plr.GetStartTime());
                try
                {
                    num = ((double) this.sellValue) / span.TotalHours;
                }
                catch
                {
                }
                int gold = this.GetGold((uint) num);
                int silver = this.GetSilver((uint) num);
                int copper = this.GetCopper((uint) num);
                uint invValue = Inventory.GetInvValue();
                Logger.WriteLine("==============================");
                Logger.WriteLine(string.Concat(new object[] { "Gold/Hour: ", gold, "g ", silver, "s ", copper, "c" }));
                Logger.WriteLine(string.Concat(new object[] { "Inventory Value: ", this.GetGold(invValue), "g ", this.GetSilver(invValue), "s ", this.GetCopper(invValue), "c   -  Free Slots: ", Inventory.GetFreeSlots() }));
                Logger.WriteLine(string.Concat(new object[] { "Loot Value: ", this.GetGold(this.sellValue), "g ", this.GetSilver(this.sellValue), "s ", this.GetCopper(this.sellValue), "c" }));
                Logger.WriteLine("==============================");
            }
        }

        private void Loot()
        {
            TERAInteract interact = null;
            int num = 1;
            if (!this.isMCLoot)
            {
                return;
            }
            Thread.Sleep(400);
            if (this.NearestAggro().IsValid)
            {
                Logger.WriteLine("[MCLoot] Aggro detected");
                return;
            }
        Label_002F:
            try
            {
                if (this.NearestAggro().IsValid)
                {
                    Logger.WriteLine("[MCLoot] Aggro detected");
                    return;
                }
                if (this.isBotStopped)
                {
                    return;
                }
                if (this.isInventoryFull())
                {
                    Logger.WriteLine("[MCLoot] Inventory FULL! Switch to standard loot.");
                    return;
                }
                Thread.Sleep(500);
                TERAInteract interact2 = this.NearestLootable();
                if (interact2 == null)
                {
                    return;
                }
                if (this.forgetable.Count > 15)
                {
                    this.forgetable.Clear();
                }
                if (interact != null)
                {
                    if (interact2.S1VillagerVolume.TERAObject.GUID == interact.S1VillagerVolume.TERAObject.GUID)
                    {
                        num++;
                        if (num <= 3)
                        {
                            goto Label_019F;
                        }
                        Logger.WriteLine("[MCLoot] Forgot: " + interact2.S1DropItemVolume.GetName());
                        this.forgetable.Add(interact2.S1VillagerVolume.TERAObject.GUID);
                        num = 1;
                        this.sellValue -= Inventory.GetSellValue(interact2.S1DropItemVolume.DropItemID);
                        goto Label_002F;
                    }
                    num = 1;
                    this.sellValue += Inventory.GetSellValue(interact2.S1DropItemVolume.DropItemID);
                }
                else
                {
                    this.sellValue += Inventory.GetSellValue(interact2.S1DropItemVolume.DropItemID);
                }
            Label_019F:
                while ((interact2.Position.TERADistance3DFromPlayer > 2.0) && (interact2.Position.TERADistance3DFromPlayer < this.maxLootDistance))
                {
                    Approach.ApproachPosition(interact2.Position, 2.0);
                    if (this.NearestAggro().IsValid)
                    {
                        Logger.WriteLine("[MCLoot] Aggro detected");
                        return;
                    }
                }
                if (this.isPlayerNearby())
                {
                    Thread.Sleep(100);
                }
                Logger.WriteLine(string.Concat(new object[] { "[MCLoot] Looting: ", interact2.S1DropItemVolume.GetName(), " Attempts: ", num }));
                interact = interact2;
                if (!interact2.Interact())
                {
                    Logger.WriteLine("[MCLoot] Forgot: " + interact2.S1DropItemVolume.GetName());
                    this.forgetable.Add(interact2.S1VillagerVolume.TERAObject.GUID);
                }
            }
            catch
            {
            }
            if (this.isPlayerNearby())
            {
                Thread.Sleep(300);
            }
            goto Label_002F;
        }

        public TERAObject NearestAggro()
        {
            List<TERAObject> list = new List<TERAObject>(MyTERA.Helpers.ObjectManager.ObjectManager.GetTERAObjects().Values);
            double maxValue = double.MaxValue;
            TERAObject invalid = TERAObject.Invalid;
            if (this.isDetectAggro)
            {
                foreach (TERAObject obj3 in list)
                {
                    if (obj3.GUID != Game.LocalTERAObject.GUID)
                    {
                        if (((obj3.IsNpc && (obj3.S1NpcGameDataController.TargetGUID == Game.LocalTERAObject.GUID)) && (obj3.S1NpcGameDataController.IsAlive && obj3.S1NpcGameDataController.InCombat)) && ((obj3.S1SkeletalMeshController.Position.TERADistance3DFromPlayer < maxValue) && (obj3.S1SkeletalMeshController.Position.TERADistance3DFromPlayer < this.maxAggroDistance)))
                        {
                            maxValue = obj3.S1SkeletalMeshController.Position.TERADistance3DFromPlayer;
                            invalid = obj3;
                        }
                        bool isPlayer = obj3.IsPlayer;
                    }
                }
            }
            return invalid;
        }

        public TERAInteract NearestLootable()
        {
            TERAInteract interact = null;
            List<TERAInteract> interactObjects = MyTERA.Helpers.InteractManager.InteractManager.GetInteractObjects();
            double maxValue = double.MaxValue;
            foreach (TERAInteract interact2 in interactObjects)
            {
                if (((interact2.IsDropItem && interact2.IsValid) && (!interact2.IsVillager && !string.IsNullOrEmpty(interact2.S1DropItemVolume.GetName()))) && ((!this.forgetable.Contains(interact2.S1VillagerVolume.TERAObject.GUID) && (interact2.Position.TERADistance3DFromPlayer < maxValue)) && ((interact2.Position.TERADistance3DFromPlayer < this.maxLootDistance) && this.isItemLootable(interact2))))
                {
                    interact = interact2;
                    maxValue = interact.Position.TERADistance3DFromPlayer;
                }
            }
            return interact;
        }

        public static void NearestObjectList()
        {
            List<TERAObject> list = new List<TERAObject>(MyTERA.Helpers.ObjectManager.ObjectManager.GetTERAObjects().Values);
            double maxValue = double.MaxValue;
            TERAObject invalid = TERAObject.Invalid;
            foreach (TERAObject obj3 in list)
            {
                if ((obj3.GUID != Game.LocalTERAObject.GUID) && (obj3.S1SkeletalMeshController.Position.TERADistance3DFromPlayer < maxValue))
                {
                    invalid = obj3;
                    maxValue = obj3.S1SkeletalMeshController.Position.TERADistance3DFromPlayer;
                }
            }
            Logger.WriteLine("Nearest object is: " + invalid.S1UserGameDataController.Name + "_" + invalid.S1NpcGameDataController.Name);
        }

        public TERAObject NearestTarget()
        {
            List<TERAObject> list = new List<TERAObject>(MyTERA.Helpers.ObjectManager.ObjectManager.GetTERAObjects().Values);
            double maxValue = double.MaxValue;
            TERAObject invalid = TERAObject.Invalid;
            if (this.isDetectAggro)
            {
                foreach (TERAObject obj3 in list)
                {
                    if (obj3.GUID != Game.LocalTERAObject.GUID)
                    {
                        if ((obj3.IsNpc && obj3.S1NpcGameDataController.IsAlive) && ((obj3.S1SkeletalMeshController.Position.TERADistance3DFromPlayer < maxValue) && (obj3.S1SkeletalMeshController.Position.TERADistance3DFromPlayer < this.maxAggroDistance)))
                        {
                            maxValue = obj3.S1SkeletalMeshController.Position.TERADistance3DFromPlayer;
                            invalid = obj3;
                        }
                        bool isPlayer = obj3.IsPlayer;
                    }
                }
            }
            return invalid;
        }

        private bool NeedHP()
        {
            return !Player.HasHPPerc(this.minHpPerc);
        }

        private bool NeedMP()
        {
            if (Player.HasMPPerc(this.minMpPerc) || (Player.isPull && this.CanPull()))
            {
                return false;
            }
            if (Player.isLong)
            {
                return !this.CanLong();
            }
            return true;
        }

        public override void OnBotStart()
        {
            this.RefreshSettings();
            this.CheckBuffs();
            this.LoadSettingsToBot();
            this.forgetable.Clear();
            this.isBotStopped = false;
            this.sellValue = 0;
            this.isCampfireInterrupt = false;
            this.isCampfireOn = false;
            this.plr = new Player();
            Player.level = Player.GetLevel();
            foreach (Structs.TERASkill skill in healSkillList)
            {
                if ((skill.GetCostMP() > this.maxHealCostMP) && skill.IsValid)
                {
                    this.maxHealCostMP = skill.GetCostMP();
                    if (this.maxHealCostMP > this.maxCostMP)
                    {
                        this.maxCostMP = this.maxHealCostMP;
                    }
                }
            }
            this.maxCostMP = (this.maxCombatCostMP + this.maxHealCostMP) + this.maxPullCostMP;
            Logger.WriteLine(string.Concat(new object[] { "MultiCombat - Pull skills : ", pullSkillList.Count, " (Max MP required: ", this.maxPullCostMP, ")" }));
            Logger.WriteLine(string.Concat(new object[] { "MultiCombat - Combat skills : ", combatSkillList.Count, " (Max MP required: ", this.maxCombatCostMP, ")" }));
            Logger.WriteLine(string.Concat(new object[] { "MultiCombat - Heal skills : ", healSkillList.Count, " (Max MP required: ", this.maxHealCostMP, ")" }));
            Logger.WriteLine("MultiCombat - Long Range skills : " + longRangeSkillList.Count);
        }

        public override void OnBotStop()
        {
            this.isBotStopped = true;
            Movements.StopMove();
            Mover.Stop();
            this.LogLootValue();
            try
            {
                this.Thread_CheckCombo.Abort();
            }
            catch
            {
            }
        }

        public override void OnLoad()
        {
            string path = new IniFile(Application.StartupPath + @"\MC_Settings.ini").ReadValue("MultiCombat", "LastProfile");
            if (File.Exists(path))
            {
                Globals.Settings = XmlSerializer.Deserialize<MultiCombat.Classes.Settings>(path);
                this.RefreshSettings();
            }
        }

        public override void OnUnload()
        {
            this.settings.Close();
        }

        public override void Patrolling()
        {
            try
            {
                this.Thread_CheckCombo.Abort();
            }
            catch
            {
            }
        }

        public override void PostCombat()
        {
            if (this.isPlayerNearby())
            {
                Logger.WriteLine(string.Concat(new object[] { "Player detected: ", this.nearestPlayer.S1UserGameDataController.Name, " at ", Math.Round(this.nearestPlayer.S1SkeletalMeshController.Position.TERADistance3DFromPlayer, 1), "m" }));
            }
            if (!this.NearestAggro().IsValid)
            {
                try
                {
                    this.Thread_CheckCombo.Abort();
                }
                catch
                {
                }
                if (!this.isBotStopped)
                {
                    try
                    {
                        this.Loot();
                    }
                    catch
                    {
                    }
                    this.LogLootValue();
                    if (this.isPlayerNearby())
                    {
                        Thread.Sleep(new Random().Next(800, 0x708));
                    }
                    if (this.NeedHP() && this.isBandage)
                    {
                        Inventory.UseBandage();
                        while (Inventory.IsBandageActive())
                        {
                            if (this.NearestAggro().IsValid || this.isBotStopped)
                            {
                                break;
                            }
                            Movements.StopMove();
                        }
                    }
                    try
                    {
                        this.Stamina();
                    }
                    catch
                    {
                    }
                    try
                    {
                        this.CheckBuffs();
                    }
                    catch
                    {
                    }
                    try
                    {
                        Inventory.UseSpeedPotion();
                    }
                    catch
                    {
                    }
                }
            }
        }

        public override bool Pull(TERAObject Object)
        {
            if (this.isPlayerNearby())
            {
                Logger.WriteLine(string.Concat(new object[] { "Player detected: ", this.nearestPlayer.S1UserGameDataController.Name, " at ", Math.Round(this.nearestPlayer.S1SkeletalMeshController.Position.TERADistance3DFromPlayer, 1), "m" }));
            }
            try
            {
                this.Thread_CheckCombo.Abort();
            }
            catch
            {
            }
            this.Thread_CheckCombo = new Thread(new ThreadStart(this.CheckFinalCombo));
            this.Thread_CheckCombo.Start();
            Player.SwitchToPull();
            if (this.DoSkillsList(pullSkillList, this.pullDist, Object))
            {
                DateTime time = DateTime.Now.AddMilliseconds(2000.0);
                while (DateTime.Now <= time)
                {
                    if (Object.S1NpcGameDataController.TargetGUID == Game.LocalTERAObject.GUID)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void RefreshSettings()
        {
            if (this.settings != null)
            {
                this.settings.Close();
                this.settings.Dispose();
            }
            this.settings = new MultiCombat.Settings();
        }

        public override void Settings()
        {
            if (this.settings != null)
            {
                this.settings.Close();
                this.settings.Dispose();
            }
            this.settings = new MultiCombat.Settings();
            this.settings.Show();
        }

        public void Stamina()
        {
            if (!this.isBotStopped)
            {
                string str = "[MC Stamina] ";
                if (!this.isStamina)
                {
                    Logger.WriteLine(str + "Stamina Process Not Enabled!");
                }
                else if (this.NearestAggro().IsValid)
                {
                    Logger.WriteLine(str + "Aggro detected. Interrupt");
                }
                else
                {
                    if (!this.isCampfireOn && !this.isCampfireInterrupt)
                    {
                        if (Player.HasStamina(this.minStamina))
                        {
                            return;
                        }
                        if (!this.usePanacea && !this.useCampfire)
                        {
                            Logger.WriteLine(str + "No Stamina Process enabled.");
                            return;
                        }
                        if (this.usePanacea && Inventory.UsePanacea())
                        {
                            Logger.WriteLine(str + "used Arunic Panacea");
                            return;
                        }
                    }
                    if (this.useCampfire)
                    {
                        if (!this.isCampfireInterrupt)
                        {
                            if (!Inventory.UseCampfire())
                            {
                                return;
                            }
                            Logger.WriteLine("[MC Stamina] Campfire Used");
                            this.isCampfireOn = true;
                            this.lastCampfire = LocalPlayer.Position;
                        }
                        if (this.lastCampfire == null)
                        {
                            Logger.WriteLine("[MC Stamina] No Campfire Around");
                        }
                        else
                        {
                            Thread.Sleep(600);
                            do
                            {
                                Approach.ApproachPosition(this.lastCampfire, 6.0);
                                if (this.NearestAggro().IsValid)
                                {
                                    Logger.WriteLine("[MC Stamina] Aggro detected. Interrupt");
                                    this.isCampfireInterrupt = true;
                                    return;
                                }
                            }
                            while (!Player.HasStamina(120));
                            NearestObjectList();
                            Logger.WriteLine("[MC Stamina] Stamina Full");
                            this.isCampfireInterrupt = false;
                            this.isCampfireOn = false;
                        }
                    }
                }
            }
        }

        public void StayInRange(TERAObject obj, int Distance)
        {
            if ((((!this.isStrafePlayer || this.isPlayerNearby()) && (this.isStrafe && !this.isBotStopped)) && !Player.isPull) && (Distance != this.meleeDist))
            {
                DateTime time = DateTime.Now.AddMilliseconds(1500.0);
                for (Position position = LocalPlayer.Position; this.GetDistanceFrom(obj) < (Distance - 2); position = LocalPlayer.Position)
                {
                    if ((!obj.IsValid || !obj.S1NpcGameDataController.IsAlive) || !obj.S1NpcGameDataController.InCombat)
                    {
                        break;
                    }
                    if (this.isBotStopped)
                    {
                        return;
                    }
                    if (DateTime.Now > time)
                    {
                        return;
                    }
                    obj.S1SkeletalMeshController.Position.Camera__Face();
                    Movements.SetMovementFlag(Enums.MovementFlags.Backward);
                    if (LocalPlayer.Position == position)
                    {
                        return;
                    }
                }
            }
        }

        public void Strafe(TERAObject obj)
        {
            if ((!this.isStrafePlayer || this.isPlayerNearby()) && (this.isStrafe && !this.isBotStopped))
            {
                int chanceDefault = this.chanceDefault;
                int num2 = this.moveMS.Next(this.minStrafeTime, this.maxStrafeTime);
                int num3 = this.moveMS.Next(0, 100);
                if ((this.castTime > 0) && this.isStrafeCharge)
                {
                    chanceDefault = this.chanceCharging;
                }
                if ((num3 <= chanceDefault) && ((Player.isCombat || Player.isLong) || (Player.isHPRegen || Player.isMPRegen)))
                {
                    if (this.moveMS.Next(0, 0x3e8) < 500)
                    {
                        this.strafeLeft = true;
                    }
                    else
                    {
                        this.strafeLeft = false;
                    }
                    string str = " for " + num2 + " ms";
                    if (this.strafeLeft)
                    {
                        Logger.WriteLine("Strafe Left" + str);
                    }
                    else
                    {
                        Logger.WriteLine("Strafe Right" + str);
                    }
                    DateTime time = DateTime.Now.AddMilliseconds((double) num2);
                    while (DateTime.Now < time)
                    {
                        if ((!obj.IsValid || !obj.S1NpcGameDataController.IsAlive) || (!obj.S1NpcGameDataController.InCombat || this.isBotStopped))
                        {
                            break;
                        }
                        obj.S1SkeletalMeshController.Position.Camera__Face();
                        if (this.strafeLeft)
                        {
                            Movements.SetMovementFlag(Enums.MovementFlags.Left);
                        }
                        else
                        {
                            Movements.SetMovementFlag(Enums.MovementFlags.Right);
                        }
                        obj.S1SkeletalMeshController.Position.Camera__Face();
                    }
                    if (!Player.IsLongRange())
                    {
                        Movements.SetMovementFlag(Enums.MovementFlags.Forward);
                        Thread.Sleep(100);
                        Movements.StopMove();
                    }
                }
            }
        }

        private bool WithinRange(TERAObject Object, int range)
        {
            return (Object.S1SkeletalMeshController.Position.TERADistance3DFromPlayer <= range);
        }

        public override string Author
        {
            get
            {
                return "manithu";
            }
        }

        public override string Name
        {
            get
            {
                return "MultiCombat v.1.1.1";
            }
        }
    }
}

