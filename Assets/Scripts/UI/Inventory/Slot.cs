using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler //, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SlotType _slotType;
    public SlotType SlotType => _slotType;
    [SerializeField] private Item _item;
    public Item item => _item;

    /// <summary>
    /// double click to use ite
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount % 2 == 0)
        {
            UseItem();
        }
    }

    /// <summary>
    /// use item
    /// </summary>
    public void UseItem()
    {
        if (item.GetItemData() != null)
        {
            if (item.GetItemData().itemType == ItemType.USEABLE && item.ThisInventoryAllData.itemsInfo[item.ThisItemIndex].itemAmount > 0)
            {
                GameManager.Instance.PlayerStatus.ApplyHealth(item.GetItemData().useableItemData.healthPoint);
                item.ThisInventoryAllData.itemsInfo[item.ThisItemIndex].itemAmount -= 1;
            }
            UpdateThisSlotItemInfo();
        }
    }

    /// <summary>
    /// update item's info in this slot by slot type
    /// </summary>
    public void UpdateThisSlotItemInfo()
    {
        // set all data in the inventory that this item in this slot belongs to
        switch (SlotType)
        {
            case SlotType.BAG:
                item.ThisInventoryAllData = InventoryManager.Instance.RuntimeBagInventoryData;
                break;

            case SlotType.WEAPON:
                item.ThisInventoryAllData = InventoryManager.Instance.RuntimeEquipmentInventoryData;
                if (item.ThisInventoryAllData.itemsInfo[item.ThisItemIndex].itemData != null)
                {
                    GameManager.Instance.PlayerStatus.ChangeWeapon(item.ThisInventoryAllData.itemsInfo[item.ThisItemIndex].itemData);
                }
                else
                {
                    GameManager.Instance.PlayerStatus.UnEquipWeapon();
                }
                break;

            case SlotType.ARMOR:
                item.ThisInventoryAllData = InventoryManager.Instance.RuntimeEquipmentInventoryData;
                break;

            case SlotType.ACTION:
                item.ThisInventoryAllData = InventoryManager.Instance.RuntimeActionBarInventoryData;
                break;
        }

        // get this item info from the inventory all data by item index
        var itemInfo = item.ThisInventoryAllData.itemsInfo[item.ThisItemIndex];// item data and item amount
        item.SetupItemInfo(itemInfo.itemData, itemInfo.itemAmount);// set this item info
    }
}
