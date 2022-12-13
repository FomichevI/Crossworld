using System.Collections.Generic;
using UnityEngine;

public class Loot
{
    public float Gold;
    public EquipmentItemData[] Items;
    public int Experience;

    public Loot(EquipmentItemData[] items, int exp, float gold)
    {
        Gold = gold;
        Experience = exp;
        Items = items;
    }
}

public class LootRamdomizer
{
    public EquipmentItemData[] GetLoot(string enemyName) //Возвращает весь выпавший с моба лут
    {
        EquipmentItemData[] lootList = null;
        float dropChance = EnemiesDataLoader.S.GetDropChance(enemyName);
        if (Random.Range(1,101) <= dropChance)
        {
            List<EquipmentItemData> allLoot = GetLootList(enemyName);
            lootList = new EquipmentItemData[1];
            lootList[0] = allLoot[Random.Range(0, allLoot.Count)]; //************Пока что только одна вещь с моба****************
        }
        return lootList;
    }

    private List<EquipmentItemData> GetLootList(string enemyName) //Возвращает все возможные вещи, которые могут выпасть с моба
    {
        string rare; 
        int minLvl;
        int maxLvl;
        EnemiesDataLoader.S.GetDropItems(enemyName, out rare, out minLvl, out maxLvl);
        return ItemDataLoader.S.GetAllItems(rare, minLvl, maxLvl);
    }
}
