namespace MultiCombat.Classes
{
    using System;

    [Serializable]
    public class Loot
    {
        public bool isGoldOnly;
        public bool isMCLoot = true;
        public bool lootCommon = true;
        public bool lootEpic = true;
        public bool lootRare = true;
        public bool lootSuperior = true;
        public int maxLootDistance = 30;
    }
}

