namespace MultiCombat.Classes
{
    using MultiCombat;
    using MyTERA.GameData;
    using MyTERA.Helpers;
    using MyTERA.Resources;
    using System;
    using System.Collections.Generic;
    using ZurasBot;

    internal class Inventory
    {
        public static bool compareNames(string s1, string s2)
        {
            string[] strArray = s1.Split(new char[] { ' ' });
            string[] strArray2 = s2.Split(new char[] { ' ' });
            if (strArray.Length > strArray2.Length)
            {
                return false;
            }
            int num = 0;
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i].Equals(strArray2[i]))
                {
                    num++;
                }
            }
            return (num == strArray.Length);
        }

        public static bool containsName(string s1, string s2)
        {
            string[] strArray = s1.Split(new char[] { ' ' });
            string[] strArray2 = s2.Split(new char[] { ' ' });
            if (strArray.Length > strArray2.Length)
            {
                return false;
            }
            int num = 0;
            for (int i = 0; i < strArray.Length; i++)
            {
                for (int j = 0; j < strArray2.Length; j++)
                {
                    if (strArray[i].Equals(strArray2[j]))
                    {
                        num++;
                    }
                }
            }
            return (num == strArray.Length);
        }

        public static Structs.TERAItem findItemByName(string itemName, bool contains)
        {
            Structs.TERAItem invalidStruct = Structs.TERAItem.InvalidStruct;
            foreach (Structs.TERAItem item2 in GetInventory())
            {
                int itemLevel = 0;
                if ((compareNames(itemName, item2.ItemData.Name) || (contains && containsName(itemName, item2.ItemData.Name))) && (((item2.ItemData.RequiredLevel <= Player.GetLevel()) && item2.IsValid) && (!item2.OnCooldown() && (item2.ItemData.ItemLevel >= itemLevel))))
                {
                    invalidStruct = item2;
                    itemLevel = item2.ItemData.ItemLevel;
                    Logger.WriteLine(string.Concat(new object[] { "[MC Inv] Found ", item2.ItemData.Name, " Level ", item2.ItemData.ItemLevel }));
                }
            }
            return invalidStruct;
        }

        public static int GetFreeSlots()
        {
            return Game.LocalTERAObject.S1InventoryController.FreeBagSlots;
        }

        public static uint GetGold()
        {
            foreach (Structs.TERAItem item in GetInventory())
            {
                Logger.WriteLine(string.Concat(new object[] { item.ItemData.ItemId, " ", item.ItemData.Name, " ", item.Count }));
                if (item.ItemData.ItemId == 0x1312d00)
                {
                    return item.Count;
                }
            }
            return 0;
        }

        public static List<Structs.TERAItem> GetInventory()
        {
            return new List<Structs.TERAItem>(Game.LocalTERAObject.S1InventoryController.GetInventoryItems().Values);
        }

        public static uint GetInvValue()
        {
            uint num = 0;
            foreach (Structs.TERAItem item in GetInventory())
            {
                uint sellValue = GetSellValue(item.Id);
                num += sellValue * item.Count;
            }
            return num;
        }

        public static uint GetSellValue(uint itemId)
        {
            DataStructureAccess<Item> item = DataManager.Data.ItemData.Item;
            for (int i = 0; i < item.Count; i++)
            {
                if (item[i].id == itemId)
                {
                    if (item[i].storeSellable)
                    {
                        return Convert.ToUInt32(item[i].sellPrice);
                    }
                    return 0;
                }
            }
            return 0;
        }

        public static bool IsBandageActive()
        {
            string itemName = "Bandage";
            return EntryPoint.isBuffActive(findItemByName(itemName, true).ItemData.Name);
        }

        public static bool UseBandage()
        {
            string name = "Bandage";
            return UseBuffItem(name, true);
        }

        public static bool UseBuffItem(string name, bool contains)
        {
            Structs.TERAItem itm = findItemByName(name, contains);
            return (!EntryPoint.isBuffActive(itm.ItemData.Name) && useItem(itm.ItemData.Name, itm));
        }

        public static bool UseCampfire()
        {
            return useItem("Campfire", Structs.TERAItem.InvalidStruct);
        }

        public static bool UseHealthtem()
        {
            string name = "Healing";
            if (UseBuffItem(name, false))
            {
                return true;
            }
            name = "Health";
            return UseBuffItem(name, false);
        }

        public static bool useItem(string itemName, Structs.TERAItem itm)
        {
            if (!LocalPlayer.IsMounted)
            {
                Structs.TERAItem item;
                if (!itm.ItemData.IsValid || !itm.IsValid)
                {
                    item = findItemByName(itemName, false);
                }
                else
                {
                    item = itm;
                }
                if (item.IsValid && ((item.Count > 0) && !item.OnCooldown()))
                {
                    Logger.WriteLine("[MC Inv] Using " + item.ItemData.Name);
                    item.Use();
                    return true;
                }
            }
            return false;
        }

        public static bool UseManaItem()
        {
            string name = "Divine Infusion";
            if (UseBuffItem(name, false))
            {
                return true;
            }
            name = "Mana";
            return UseBuffItem(name, false);
        }

        public static bool UsePanacea()
        {
            return useItem("Arunic Panacea", Structs.TERAItem.InvalidStruct);
        }

        public static bool UseSpeedPotion()
        {
            string name = "Speed Potion";
            return UseBuffItem(name, false);
        }
    }
}

