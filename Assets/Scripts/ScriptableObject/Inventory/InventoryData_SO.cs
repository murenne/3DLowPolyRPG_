using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItemInfo> itemsInfo = new();

    public void AddItems(ItemData_SO newItemData, int amount)
    {
        bool isFound = false;

        if (newItemData.isStackable)
        {
            foreach (var itemInfo in itemsInfo)
            {
                if (itemInfo.itemData == newItemData)
                {
                    itemInfo.itemAmount += amount;
                    isFound = true;
                    break;
                }
            }
        }

        for (int i = 0; i < itemsInfo.Count; i++)
        {
            if (itemsInfo[i].itemData == null && !isFound)
            {
                itemsInfo[i].itemData = newItemData;
                itemsInfo[i].itemAmount = amount;
                break;
            }
        }

    }
}

[System.Serializable]
public class InventoryItemInfo
{
    public ItemData_SO itemData;
    public int itemAmount;
}